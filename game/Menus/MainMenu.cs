using Godot;
using System;

namespace ldjam50.Menus
{
    public class MainMenu : Node2D
    {
        private const String ButtonContainerPath = "CanvasLayer/PanelContainer/VBoxContainer/HBoxContainer/CenterContainer/VBoxContainer/";

        public override void _Ready()
        {
            GetNode<TextureButton>(ButtonContainerPath + "Start").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Start });
            GetNode<TextureButton>(ButtonContainerPath + "Instructions").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Instructions });
            GetNode<TextureButton>(ButtonContainerPath + "Credits").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Credits });
            GetNode<TextureButton>(ButtonContainerPath + "Exit").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Exit });
            GetNode<Button>(ButtonContainerPath + "HBoxContainer/Andrew").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Andrew });
            GetNode<Button>(ButtonContainerPath + "HBoxContainer/Adam").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Adam });
            GetNode<Button>(ButtonContainerPath + "HBoxContainer/Michael").Connect("pressed", this, nameof(Pressed), new Godot.Collections.Array { ButtonID.Michael });
        }

        private void Pressed(ButtonID id)
        {
            switch (id)
            {
                case ButtonID.Start:
                    "Start".Print();
                    //Global.SceneManager.Scene = SceneID.
                    break;
                case ButtonID.Instructions:
                    "Instructions".Print();
                    //Global.SceneManager.Scene = SceneID.
                    break;
                case ButtonID.Credits:
                    "Credits".Print();
                    //Global.SceneManager.Scene = SceneID.
                    break;
                case ButtonID.Exit:
                    GetTree().Quit();
                    break;
                case ButtonID.Andrew:
                    Global.SceneManager.Scene = SceneID.AndrewScene;
                    break;
                case ButtonID.Adam:
                    Global.SceneManager.Scene = SceneID.AdamScene;
                    break;
                case ButtonID.Michael:
                    Global.SceneManager.Scene = SceneID.MichaelScene;
                    break;
                default:
                    GD.PushWarning($"Button {id} has not been implemented");
                    break;
            }
        }

        private enum ButtonID
        {
            Start,
            Instructions,
            Credits,
            Exit,
            Andrew,
            Adam,
            Michael,
        };
    }
}
