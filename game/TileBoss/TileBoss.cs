using Godot;
using ldjam50.Entities;
using ldjam50.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ldjam50.TileBoss
{
    public delegate Boolean CanHitQuery();
    public record TileInfo(Vector2 Position, TileType Type, Single HP, CanHitQuery CanHit);

    public enum TileType
    {
        Adam,
        Hull,
        Gun,
    }

    public class TileBoss : Node2D, IMovable
    {
        public MovementComponent Movement => _Movement ??= GetNode<MovementComponent>("MovementComponent") ?? throw new Exception("No movement component on tileboss");
        private MovementComponent _Movement;
        public AudioStreamPlayer2D AudioPlayer => _AudioPlayer ??= GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D") ?? throw new Exception("No audio player on tileboss");
        private AudioStreamPlayer2D _AudioPlayer;

        public List<T> Guns<T>() where T : BossGun => Ship.GetChildren().ToList<T>();

        const Int32 Autotile = 2;
        const Int32 CoreTile = 1;
        const Single BuildSpeed = 0.1f;

        [Obsolete]
        public void Fire(Vector2 somethign) { }

        public TileMap Ship => _Ship ??= GetNode<TileMap>("Ship") ?? throw new Exception("Ship not found on tileboss");
        private TileMap _Ship;

        private TileMap GetMap(Int32 phase) => GetNode<TileMap>($"{phase:00}") ?? throw new Exception($"Phase {phase:00} not found on tileboss");
        private readonly List<Vector2> BuildQueue = new();

        private readonly List<TileInfo> Info = new();

        public Int32 Phase { get; private set; } = 2;

        public Vector2 MoveDirection;

        private Time Time;

        /// <summary>
        /// Collide with tileboss
        /// </summary>
        /// <param name="position">Global position of collision</param>
        /// <param name="damage">Amount of HP to remove</param>
        public void Collide(Vector2 position, Single damage)
        {
            var tile = Info.OrderBy(i => (Ship.MapToWorld(i.Position) + new Vector2(18f, 18f)).DistanceSquaredTo(Ship.ToLocal(position))).First().Position;

            var info = Info.FirstOrDefault(i => i.Position == tile);

            if (info is not null && info.CanHit())
            {
                if (info.HP - damage <= 0f)
                {
                    Ship.SetCellv(tile, TileMap.InvalidCell);
                    Ship.UpdateBitmaskArea(tile);
                    if (info.Type == TileType.Gun)
                    {
                        Ship.RemoveChild(Ship.GetChildren().ToList<BossGun>().First(g => Ship.WorldToMap(g.Position) == info.Position));
                    }
                }
                else
                {
                    Info.Add(info with { HP = info.HP - 1f });
                }

                Info.Remove(info);
            }
        }

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Time = Global.Time;
        }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Movement.MaxSpeed = new Vector2(2, 2);
            BuildShip();
        }

        public override void _PhysicsProcess(Single delta)
        {
            if (MoveDirection != null && (MoveDirection.x != 0 || MoveDirection.y != 0)) {
                var bounds = GetNode<TileMap>("Ship").GetUsedRect();

                if (MoveDirection.x < 0f && GlobalPosition.x + bounds.Position.x * 36 < 925)
                {
                    MoveDirection.x = 0f;
                }
                if (MoveDirection.x > 0f && GlobalPosition.x + bounds.End.x * 100 > 1920 - 25)
                {
                    MoveDirection.x = 0f;
                }
                if (MoveDirection.y < 0f && GlobalPosition.y + bounds.Position.y * 36 < 0 + 30)
                {
                    MoveDirection.y = 0f;
                }

                if (MoveDirection.y > 0f && GlobalPosition.y + bounds.End.y * 36 > 1080 - 30)
                {
                    MoveDirection.y = 0f;
                }
            }
            Movement.TargetDirection = MoveDirection;
        }

        ///// <summary>
        ///// Input
        ///// </summary>
        ///// <param name="e"></param>
        //public override void _Input(InputEvent e)
        //{
        //    if (e is InputEventMouseButton emb && emb.Pressed is true)
        //    {
        //        var tile = Ship.WorldToMap(Ship.ToLocal(GetGlobalMousePosition()));
        //        var info = Info.FirstOrDefault(i => i.Position == tile);
        //        if (info is not null && info.CanHit())
        //        {
        //            if (info.HP - 1f == 0f)
        //            {
        //                Ship.SetCellv(tile, TileMap.InvalidCell);
        //                Ship.UpdateBitmaskArea(tile);
        //                if (info.Type == TileType.Gun)
        //                {
        //                    Ship.RemoveChild(Ship.GetChildren().ToList<BossGun>().First(g => Ship.WorldToMap(g.Position) == info.Position));
        //                }
        //            }
        //            else
        //            {
        //                Info.Add(info with { HP = info.HP - 1f });
        //            }

        //            Info.Remove(info);
        //        }
        //    }
        //}

        /// <summary>
        /// Builds the ship for the current phase
        /// </summary>
        public void BuildShip()
        {
            Ship.Clear();
            Info.Clear();

            GetMap(Phase).GetUsedCells().ToList<Vector2>().ForEach(c => BuildQueue.Add(c));

            Ship.SetCellv(Vector2.Zero, CoreTile);
            BuildQueue.Remove(Vector2.Zero);
            Info.Add(new TileInfo(Vector2.Zero, TileType.Adam, 3f, () => Info.All(r => r.Type != TileType.Gun)));
            AudioPlayer.Play();

            Time.AddNotify(1f, BuildNext);
        }

        /// <summary>
        /// Adds the next piece to the ship
        /// </summary>
        private void BuildNext()
        {
            var built = Ship.GetUsedCells().ToList<Vector2>();

            var ableToAdd = (from i in BuildQueue
                             where built.Any(c => IsAdjacent(i, c))
                             select i).ToList();

            if (ableToAdd.Count is 0)
            {
                GD.PushWarning("Unable to find adjacent tile but there are still tiles to build");
                return;
            }

            var toAdd = ableToAdd[ableToAdd.Count is 1 ? 0 : (Int32)(GD.Randi() % (ableToAdd.Count - 1))];

            Ship.SetCellv(toAdd, Autotile);
            Ship.UpdateBitmaskArea(toAdd);
            AudioPlayer.Play();
            var gun = GetMap(Phase).GetChildren().ToList<BossGun>().FirstOrDefault(g => Ship.WorldToMap(g.Position) == toAdd);

            if (gun is not null)
            {
                gun.GetParent().RemoveChild(gun);
                Ship.AddChild(gun);
                Info.Add(new TileInfo(toAdd, TileType.Gun, 2f, () => true));
            }
            else
            {
                Info.Add(new TileInfo(toAdd, TileType.Hull, 1f, () => true));
            }

            BuildQueue.Remove(toAdd);
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