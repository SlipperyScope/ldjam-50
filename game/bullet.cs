using Godot;
using System;
using ldjam50;
using ldjam50.Entities;
using ldjam50.TileBoss;

public class Bullet : Area2D
{
    public Vector2 Velocity {get; set;}
    public Boolean FromPlayer{get; set;}

    public Single Damage { get; set; } = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Velocity = new Vector2(900, 0);
        
    }
    public override void _EnterTree()
    {
        Connect("area_entered", this, nameof(OnAreaEntered));
        Connect("body_entered", this, nameof(OnBodyEntered));
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
        if ((!FromPlayer && other is HeroSprite) || (FromPlayer && other is TileBoss)) {
            QueueFree();
        }
    }

    public void OnBodyEntered(Node body)
    {
        switch(body)
        {
            case TileMap tilemap when tilemap.Name == "Ship" && FromPlayer is true:
                tilemap.GetParent<TileBoss>().Collide(GlobalPosition + Velocity.Normalized() + Velocity.Normalized() * 10f/*collider radius*/, Damage);
                break;
        }
    }
}
