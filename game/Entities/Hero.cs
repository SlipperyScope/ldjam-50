using Godot;
using ldjam50.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Entities
{
    public class Hero : Area2D, IMovable
    {
        public MovementComponent Movement => _Movement ??= GetNode<MovementComponent>("MovementComponent") ?? throw new Exception("No movement component on hero");
        private MovementComponent _Movement;

        public PlayerController Controller => _Controller ??= GetNode<PlayerController>("Controller") ?? throw new Exception("No controller found on hero");
        private PlayerController _Controller;

        public Sprite Sprite => _Sprite ??= GetNode<Sprite>("Sprite") ?? throw new Exception("No sprite, yo");
        private Sprite _Sprite;

        /// <summary>
        /// Physics process
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(Single delta)
        {
            var input = Controller.InputVector;

            Sprite.LookAt(Position + input);
            
            Movement.TargetDirection = Controller.InputVector;
        }
    }
}
