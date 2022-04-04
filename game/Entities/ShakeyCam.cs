using Godot;
using System;

namespace ldjam50.Entities
{
    public class ShakeyCam : Node2D
    {
        private readonly Vector2 BaselinePower = new(10f, 10f);
        private readonly Vector2 BaselineSpeed = new(0.5f, 0.5f);
        private const Single Snappiness = 0.05f;

        private Vector2 TargetPower = Vector2.Zero;
        private Vector2 TargetSpeed = Vector2.Zero;

        public Vector2 Power { get; set; }
        public Vector2 Speed { get; set; }

        private Time Time;

        public override void _EnterTree()
        {
            Time = Global.Time;
            Speed = TargetSpeed = BaselineSpeed;
            Power = TargetPower = BaselinePower;
        }

        public override void _Ready()
        {
            ModVars();
        }

        public override void _Process(Single delta)
        {
            var position = Position;

            position.x = Power.x * Mathf.Cos(Time.Seconds * Speed.x);
            position.y = Power.y * Mathf.Sin(Time.Seconds * Speed.y);

            Position = (Position - new Vector2(512f, 512f)).LinearInterpolate(position, 0.1f) + new Vector2(512f, 512f);

            Power = Power.LinearInterpolate(TargetPower, Snappiness);
            Speed = Speed.LinearInterpolate(TargetSpeed, Snappiness);
        }

        public override void _Input(InputEvent e)
        {
            if (e is InputEventKey ek && ek.Pressed && ek.Scancode == (uint)KeyList.Enter)
            {
                Shake();
            }
        }

        public void Shake()
        {
            Power += new Vector2(Rand(-100f, 100f), Rand(-100f, 100f));
            Speed = BaselineSpeed + new Vector2(Rand(2f, 5f), Rand(2f, 5f));
        }

        private void ModVars()
        {
            TargetPower = BaselinePower + new Vector2(Rand(-1f, 1f), Rand(-1f, 1f));
            TargetSpeed = BaselineSpeed + new Vector2(Rand(-0.02f, 0.02f), Rand(-0.02f, 0.02f));
            
            Time.AddNotify(3f, ModVars);
        }

        private static Single Rand(Single from, Single to) => (Single)GD.RandRange(from, to);
    }
}