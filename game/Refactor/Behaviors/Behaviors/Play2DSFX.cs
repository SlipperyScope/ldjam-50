using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Behaviors
{
    /// <summary>
    /// Plays an audio effect
    /// <para>Pass: Audio played
    /// <br />Fail: Not possible
    /// <br />Abort: Invalid audio player or stream
    /// </para>
    /// </summary>
    public class Play2DSFX : Behavior
    {
        [Export]
        private NodePath Player;
        private AudioStreamPlayer2D AudioPlayer => _AudioPlayer ??= GetNode<AudioStreamPlayer2D>(Player);
        private AudioStreamPlayer2D _AudioPlayer;

        [Export]
        private AudioStream SFX;
        
        public override void Execute(IRobot robot)
        {
            if (AudioPlayer is null || SFX is null)
            {
                if (AudioPlayer is null) "Player is null".Warn();
                if (SFX is null) "SFX is null".Warn();
                Abort();
            }
            else
            {
                if (AudioPlayer.Stream != SFX) AudioPlayer.Stream = SFX;
                AudioPlayer.Play();
                Finish(true);
            }
        }
    }
}
