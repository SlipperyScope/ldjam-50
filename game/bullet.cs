using Godot;
using System;

public class Bullet : Area2D
{
    public Vector2 Velocity {get; set;}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Velocity = new Vector2(200, 0);
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += Velocity * delta;
    }
}
