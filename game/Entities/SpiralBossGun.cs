using Godot;
using System;

namespace ldjam50.Entities
{
    public class SpiralBossGun : BossGun
    {
        const String BulletScenePath = "res://Bullet.tscn";
        public PackedScene BulletScene;
        public override void Shoot(Vector2 direction)
        {
            var bullet = BulletScene.Instance<Bullet>();
            GetTree().Root.AddChild(bullet);
            bullet.Position = this.GlobalPosition;
            bullet.Velocity = direction * 700;
        }

        public override void _Ready()
        {
            BulletScene = GD.Load<PackedScene>(BulletScenePath);
        }
    }
}
