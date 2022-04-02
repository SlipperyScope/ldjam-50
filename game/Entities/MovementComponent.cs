using Godot;
using ldjam50.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Entities
{
    public class MovementComponent : Node
    {
        #region Config
        private const Single InterpSpeed = 0.1f;
        #endregion

        /// <summary>
        /// Movement speed in m/s
        /// </summary>
        [Export]
        public Single MaxSpeed { get; set; } = 10f;

        /// <summary>
        /// Sets the direction of travel at max speed
        /// </summary>
        public Vector2 TargetDirection
        {
            get => TargetVelocity.Normalized();
            set => TargetVelocity = value * MaxSpeed;
        }

        /// <summary>
        /// Target velocity in m/s
        /// </summary>
        public Vector2 TargetVelocity
        { 
            get => _TargetVelocity; 
            set => _TargetVelocity = value.Clamped(MaxSpeed); 
        }
        private Vector2 _TargetVelocity;

        /// <summary>
        /// Current velocity in m/s
        /// </summary>
        public Vector2 Velocity
        {
            get => _Velocity;
            set
            {
                _Velocity = value;
                GD.PushWarning("Did you mean to set target velocity?"); // Here you go, Adam
            }
        }
        private Vector2 _Velocity;
        
        private IMovable Movable;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Movable = GetParent<IMovable>();
            if (Movable is null)
            {
                GD.PushWarning("Movement component is not attached to an IMovable");
                QueueFree();
            }
        }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {

        }

        /// <summary>
        /// Physics Process
        /// </summary>
        public override void _PhysicsProcess(Single delta)
        {
            var vel = Velocity;
            var pos = Movable.Position;

            vel = vel.LinearInterpolate(TargetVelocity, InterpSpeed);

            Movable.Position = pos + vel * Global.PxPM * delta;

            _Velocity = vel;
        }
    }
}
