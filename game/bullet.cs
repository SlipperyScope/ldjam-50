using Godot;
using System;
using ldjam50;
using ldjam50.Entities;

public class Bullet : Area2D
{
    public Vector2 Velocity {get; set;}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Velocity = new Vector2(900, 0);
        
    }
    public override void _EnterTree()
    {
        Connect("area_entered", this, nameof(OnAreaEntered));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += Velocity * delta;
        if (Position.x < Extensions.ScreenLeft(this) || Position.x > Extensions.ScreenRight(this)
            || Position.y < Extensions.ScreenTop(this) || Position.y > Extensions.ScreenBottom(this))
        {
            QueueFree();
        }
    }

    public void OnAreaEntered(Area2D other) {
        if (other is HeroSprite) {
            QueueFree();
        }
    }
}
