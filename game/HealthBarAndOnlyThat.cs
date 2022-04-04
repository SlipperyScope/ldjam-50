using Godot;
using System;
using ldjam50.Entities;

public class HealthBarAndOnlyThat : Node
{
    [Export]
    protected NodePath Hero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var hero = GetNode<Hero>(Hero).GetNode<HeroSprite>("Area2D");
        hero.Ouchies += UpdateHealth;
    }

    void UpdateHealth(object sender, OuchiesArgs e) {
        GD.Print($"Welp, ouch: {e.Health}");
        for (var i = 1; i <= 6; i++) {
            var rect = GetNode<ColorRect>($"h{i}");
            if (i < e.Health) rect.Visible = true;
            else rect.Visible = false;
        }
    }
}
