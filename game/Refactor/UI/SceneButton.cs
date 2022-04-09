using Godot;
using ldjam50;
using System;

namespace ldjam50.Refactor.UI
{
    public class SceneButton : Button
    {
        [Export]
        private SceneID Scene;

        public override void _EnterTree()
        {
            Connect("pressed", Global.SceneManager, nameof(Global.SceneManager.ChangeScene), new Godot.Collections.Array(Scene));
        }
    }
}