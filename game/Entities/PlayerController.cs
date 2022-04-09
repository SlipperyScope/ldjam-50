using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50
{
    public class PlayerController : Node
    {
        public Vector2 InputVector { get; private set; } = Vector2.Zero;

        public Boolean Up => InputVector.y < 0f;
        public Boolean Down => InputVector.y > 0f;
        public Boolean Left => InputVector.x < 0f;
        public Boolean Right => InputVector.x > 0f;
        public Boolean Shoot { get; private set; } = false;

        public override void _Process(Single delta)
        {
            var input = Vector2.Zero;
            if (Input.IsActionPressed(Global.InputActions[InputAction.Up])) input.y -= 1f;
            if (Input.IsActionPressed(Global.InputActions[InputAction.Down])) input.y += 1f;
            if (Input.IsActionPressed(Global.InputActions[InputAction.Left])) input.x -= 1f;
            if (Input.IsActionPressed(Global.InputActions[InputAction.Right])) input.x += 1f;
            InputVector = input;

            Shoot = Input.IsActionPressed(Global.InputActions[InputAction.Fire]);
        }
    }
}
