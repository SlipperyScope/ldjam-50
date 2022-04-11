using Godot;
using ldjam50.Refactor.Entities.BigBad.Templates;
using ldjam50.Refactor.Utils;
using ldjam50.Refactor.Utils.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Entities.BigBad
{

    /// <summary>
    /// Hull of a big bad 
    /// </summary>
    public class BigBadHull : TileMap
    {
        public event HullEventHandler HullEvent;

        [Export]
        private Int32 HullHP = 1;

        [Export]
        private Int32 GunHP = 2;

        [Export]
        private Int32 CoreHP = 3;

        private readonly List<Cell> BuildQueue = new();

        /// <summary>
        /// Builds a hull from a template
        /// </summary>
        /// <param name="template"></param>
        public void Build(BigBadTemplate template)
        {
            Clear();
            SetCellv(Vector2.Zero, template.TileSet.Tiles.First(t => t.Name == "Core").ID);

            var cells = new List<Cell>();

            Int32 x, y;

            foreach (Tile tile in template.TileSet)
            {
                foreach (Vector2 coord in template.GetUsedCellsById(tile.ID))
                {
                    x = (Int32)coord.x;
                    y = (Int32)coord.y;
                    cells.Add(new Cell
                    {
                        Tile = tile,
                        Coordinate = coord,
                        FlipX = template.IsCellXFlipped(x, y),
                        FlipY = template.IsCellYFlipped(x, y),
                        Transpose = template.IsCellTransposed(x, y),
                        AutotileCoordinate = template.GetCellAutotileCoord(x, y),
                    });
                }
            }

            cells.Where(c => c.Tile.Name == "Core").ToList().Shuffled().ForEach(c => BuildQueue.Add(c));
            cells.Where(c => c.Tile.Name != "Core").ToList().Shuffled().ForEach(c => BuildQueue.Add(c));

            HullEvent?.Invoke(this, new HullEventArgs(HullAction.StartBuild));
            Global.Time.AddRecurring(0f, 0.01f, (UInt32)BuildQueue.Count, BuildNext);
        }

        /// <summary>
        /// Builds the next tile
        /// </summary>
        private void BuildNext()
        {
            var cell = BuildQueue.FirstOrDefault(c => c.Tile.Name == "Core" || GetUsedCells().ToList<Vector2>().Any(v => v.AjacentTo(c.Coordinate)));
            
            if (cell is null)
            {
                $"There are {BuildQueue.Count} more cells but none are adjacent".Warn();
            }
            else
            {
                BuildQueue.Remove(cell);
                SetCell(cell.x, cell.y, cell.Tile.ID, cell.FlipX, cell.FlipY, cell.Transpose, cell.AutotileCoordinate);
                UpdateBitmaskArea(cell.Coordinate);

                Global.SFX.Play(Sample.BB_Build, ToGlobal(MapToWorld(cell.Coordinate)));

                if (BuildQueue.Count == 0)
                {
                    HullEvent?.Invoke(this, new HullEventArgs(HullAction.CompleteBuild));
                }
            }
        }
    }

    public delegate void HullEventHandler(object sender, HullEventArgs e);

    /// <summary>
    /// Arguments for a hull event
    /// </summary>
    public class HullEventArgs : EventArgs
    {
        public HullEventArgs(HullAction hullAction)
        {
            HullAction = hullAction;
        }

        public HullAction HullAction { get; set; }
    }

    /// <summary>
    /// Hull Action
    /// </summary>
    public enum HullAction
    {
        StartBuild,
        CompleteBuild
    }
}
