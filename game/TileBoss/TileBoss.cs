using Godot;
using ldjam50.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ldjam50.TileBoss
{
    public class TileBoss : Node2D, IMovable
    {
        const Int32 Autotile = 0;
        const Int32 CoreTile = 1;
        const Single BuildSpeed = 0.0f;

        public TileMap Ship => _Ship ??= GetNode<TileMap>("Ship") ?? throw new Exception("Ship not found on tileboss");
        private TileMap _Ship;

        private TileMap GetMap(Int32 phase) => GetNode<TileMap>($"{phase:00}") ?? throw new Exception($"Phase {phase:00} not found on tileboss");
        private List<Vector2> BuildQueue = new();

        public Int32 Phase { get; private set; } = 3;

        private Time Time;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Time = Global.Time;
        }

        public override void _Ready()
        {
            BuildShip();
        }

        /// <summary>
        /// Builds the ship for the current phase
        /// </summary>
        public void BuildShip()
        {
            Ship.Clear();
            GetMap(Phase).GetUsedCells().ToList<Vector2>().ForEach(c => BuildQueue.Add(c));
            Ship.SetCellv(Vector2.Zero, CoreTile);
            BuildQueue.Remove(Vector2.Zero);
            Time.AddNotify(1f, BuildNext);
        }

        /// <summary>
        /// Adds the next piece to the ship
        /// </summary>
        private void BuildNext()
        {var built = Ship.GetUsedCells().ToList<Vector2>();

            var ableToAdd = (from i in BuildQueue
                             where built.Any(c => IsAdjacent(i, c))
                             select i).ToList();

            if (ableToAdd.Count is 0)
            {
                GD.PushWarning("Unable to find adjacent tile");
                return;
            }

            var index = ableToAdd.Count is 1 ? 0 : (Int32)(GD.Randi() % (ableToAdd.Count - 1));

            Ship.SetCellv(ableToAdd[index], Autotile);
            Ship.UpdateBitmaskArea(ableToAdd[index]);
            BuildQueue.Remove(ableToAdd[index]);

            if (BuildQueue.Count > 0)
            { 
                Time.AddNotify(BuildSpeed, BuildNext);
            }
        }

        /// <summary>
        /// Determines if two vectors are adjacent
        /// </summary>
        /// <param name="first">Vector</param>
        /// <param name="second">Other vector</param>
        /// <returns>True if they are adjacent</returns>
        private static Boolean IsAdjacent(Vector2 first, Vector2 second)
        {
            var diff = first - second;
            return diff == Vector2.Up || diff == Vector2.Down || diff == Vector2.Left || diff == Vector2.Right;
        }
    }
}