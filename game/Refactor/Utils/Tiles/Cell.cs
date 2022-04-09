using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Utils.Tiles
{
    /// <summary>
    /// Data representing a cell in a tilemap
    /// </summary>
    public record Cell
    {
        public Tile Tile { get; set; }
        public Vector2 Coordinate { get; set; }
        public Int32 x => (Int32)Coordinate.x;
        public Int32 y => (Int32)Coordinate.y;
        public Boolean FlipX { get; set; }
        public Boolean FlipY { get; set; }
        public Boolean Transpose { get; set; }
        public Vector2 AutotileCoordinate { get; set; }
    }
}