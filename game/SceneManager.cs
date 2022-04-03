using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ldjam50
{
    public class SceneManager : Node
    {
        public SceneID PreviousScene { get; private set; }
        public SceneID Scene
        {
            get => _Scene;
            set => ChangeScene(value);
        }
        private SceneID _Scene;

        public void ChangeScene(SceneID id)
        {
            PreviousScene = Scene;
            _Scene = id;
            GetTree().ChangeScene(SceneMap[id]);
        }

        /// <summary>
        /// Map of scene IDs to scene paths
        /// </summary>
        public static readonly Dictionary<SceneID, String> SceneMap = new()
        {
            { SceneID.MainMenu, "res://Menus/MainMenu.tscn" },
            { SceneID.AndrewScene, "res://Levels/AndrewLevel.tscn" },
            { SceneID.AdamScene, "res://Levels/Adumb.tscn" },
            { SceneID.MichaelScene, "" },
            { SceneID.BattleScene, "res://Levels/Battle.tscn" }
        };
    }

    public enum SceneID
    {
        MainMenu,
        AndrewScene,
        AdamScene,
        MichaelScene,
        BattleScene
    }
}
