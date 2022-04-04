using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Entities
{
    [Tool]
    public abstract class BossGun : Node2D
    {
        [Export]
        protected Texture Texture
        {
            get => _Texture;
            set
            {
                _Texture = value;
                if (Engine.EditorHint is true && value is not null)
                {
                    GetNode<Sprite>("Sprite").Texture = Texture;
                }
            }
        }
        private Texture _Texture;

        [Export]
        protected PackedScene Projectile;

        protected Sprite Sprite => _Sprite ??= GetNode<Sprite>("Sprite") ?? throw new System.Exception("No Sprite on boss gun");
        private Sprite _Sprite;

        protected AudioStreamPlayer2D AudioPlayer => _AudioPlayer ??= GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D") ?? throw new System.Exception("No audio player on boss gun");
        
        private AudioStreamPlayer2D _AudioPlayer;

        public override void _Ready()
        {
            Sprite.Texture = Texture;
        }

        public virtual void Shoot(Vector2 direction)
        {
            var bullet = Projectile.Instance<Bullet>();
            GetTree().Root.AddChild(bullet);
            bullet.Position = this.GlobalPosition;
            bullet.Velocity = direction * 700;

            PlaySound();
        }

        protected void PlaySound() => AudioPlayer.Play();
    }
}
