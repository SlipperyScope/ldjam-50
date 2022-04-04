using Godot;
using System;

public class Portrait : Control
{
    Label Textbox;
    public override void _Ready()
    {
        Textbox = GetNode<Label>("Label");
    }

    public void Say(string line = "") {
        Visible = true;
        Textbox.Text = line;
    }

    public void ExitStageLeft() {
        Visible = false;
    }
}
