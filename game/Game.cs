using Godot;
using System;

namespace ldjam50
{
    public class Game : Node2D
    {
        public override void _Ready()
        {
            Global.SceneManager.Scene = SceneID.MainMenu;
        }
    }
}
