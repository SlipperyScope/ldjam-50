using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Entities
{
    public abstract class BossGun : Node2D
    {
        protected AudioStreamPlayer2D AudioPlayer => _AudioPlayer ??= GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D") ?? throw new System.Exception("No audio player on boss gun");
        private AudioStreamPlayer2D _AudioPlayer;
        public abstract void Shoot(Vector2 dir);

        protected void PlaySound() => AudioPlayer.Play();
    }
}
