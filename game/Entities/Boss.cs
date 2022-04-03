using Godot;
using System;
using ldjam50.Interfaces;

public class Boss : Area2D, IMovable
{
    const String BulletScenePath = "res://Bullet.tscn";
    public PackedScene BulletScene;

    public override void _Ready()
    {
        BulletScene = GD.Load<PackedScene>(BulletScenePath);
    }

    public void Fire(Vector2 direction)
    {
        var bullet = BulletScene.Instance<Bullet>();
        GetTree().Root.AddChild(bullet);
        bullet.Position = this.Position;
        bullet.Velocity = direction * 700;
    }
}
