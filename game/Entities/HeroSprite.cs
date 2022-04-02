using Godot;
using ldjam50.Interfaces;
using System;

namespace ldjam50.Entities
{
    public class HeroSprite : Sprite, IMovable
    {
        public MovementComponent Movement => _Movement ??= GetNode<MovementComponent>("MovementComponent") ?? throw new Exception("No movement component on hero");
        private MovementComponent _Movement;

        public PlayerController Controller => _Controller ??= GetNode<PlayerController>("Controller") ?? throw new Exception("No controller found on hero");
        private PlayerController _Controller;

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            //Rotation = Mathf.Pi * -0.5f;
        }

        public override void _Ready()
        {
            Movement.InterpSpeed = new(0.3f, 0.3f);
        }

        /// <summary>
        /// Physics Process
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(Single delta)
        {
            var input = Controller.InputVector;
            var screenStart = GetParent<Node2D>().ToLocal(this.ScreenRect().Position);
            var screenEnd = GetParent<Node2D>().ToLocal(this.ScreenRect().End);
            var bounds = GetRect();
            var position = Position;

            if (input.x < 0f && position.x + bounds.Position.x < screenStart.x + Global.ScreenBuffer)
            {
                input.x = 0f;
                //position.x = (screenStart.x + Global.ScreenBuffer) - bounds.Position.x;
            }

            if (input.y < 0f && position.y + bounds.Position.y < screenStart.y + Global.ScreenBuffer)
            {
                input.y = 0f;
            }

            if (input.y > 0f && position.y + bounds.End.y > screenEnd.y - Global.ScreenBuffer)
            {
                input.y = 0f;
            }

            Movement.TargetDirection = input;
        }
    }
}
