using Godot;
using System;
using System.Collections.Generic;

namespace ldjam50
{
    public class Global : Node2D
    {
        public static Global Instance { get; private set; }
        public static Time Time { get; private set; }
        public static SceneManager SceneManager { get; private set; }

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Instance = this;
        }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            AddChild(Time = new Time());
            AddChild(SceneManager = new SceneManager());
        }

        #region Constants

        /// <summary>
        /// Pixels per meter
        /// </summary>
        public const Single PxPM = 100f;

        /// <summary>
        /// Distance from the screen edge player can move to
        /// </summary>
        public const Single ScreenBuffer = 10f;

        #endregion

        #region Actions

        /// <summary>
        /// Input actions
        /// </summary>
        public static readonly Dictionary<InputAction, String> InputActions = new()
        {
            { InputAction.Up, "Up" },
            { InputAction.Down, "Down" },
            { InputAction.Left, "Left" },
            { InputAction.Right, "Right" },
            { InputAction.Space, "Space" },
            { InputAction.Use, "Use"},
            { InputAction.Click, "Click" },
        };

        #endregion
    }

    /// <summary>
    /// Input actions
    /// </summary>
    public enum InputAction
    {
        Up,
        Down,
        Left,
        Right,
        Space,
        Click,
        Use,
    }
}
