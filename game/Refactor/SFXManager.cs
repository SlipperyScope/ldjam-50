using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor
{
    /// <summary>
    /// Manages Sound effects
    /// </summary>
    public class SFXManager : Node2D
    {
        const String SFXPath = "res://Refactor/Audio/SFX/";

        /// <summary>
        /// Audio stream refs by ID
        /// </summary>
        private Dictionary<Sample, AudioStream> Streams = new();

        /// <summary>
        /// Audio players
        /// </summary>
        private List<AudioStreamPlayer2D> Players = new();

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            Preload();

            for (var i = 0; i < 15; i++)
            {
                var player = new AudioStreamPlayer2D();
                Players.Add(player);
                AddChild(player);
            }
        }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Global.Time.AddLooping(10f, Cleanup);
        }

        /// <summary>
        /// Removes extra empty players
        /// </summary>
        private void Cleanup()
        {
            var count = Players.Count;

            foreach (var player in Players.Where(p => p.Playing is false).ToList())
            {
                if (count > 15)
                {
                    Players.Remove(player);
                    player.Kill();
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Plays a sample at the center of the screen
        /// </summary>
        /// <param name="sample">Sample to play</param>
        public void Play(Sample sample) => Play(sample, (this.ScreenRect().Position + this.ScreenRect().End) / 2f);

        // TODO: Probably optimize this or whatever
        /// <summary>
        /// Plays a sample at a location
        /// </summary>
        /// <param name="sample">Sample to play</param>
        /// <param name="globalPosition">Location to play at in global coordinates</param>
        public void Play(Sample sample, Vector2 globalPosition)
        {
            var player = Players.FirstOrDefault(p => p.Playing is false);
            if (player is not null)
            {

                //"played empty".Print();
                player.Stream = Streams[sample];
                player.Position = globalPosition;
                player.Play();
            }
            else
            {
                player = Players.Where(p => p.Playing is true && p.Stream == Streams[sample]).OrderBy(s => s.GetPlaybackPosition()).FirstOrDefault();
                if (player is not null)
                {
                    //"played existing".Print();
                    player.Position = globalPosition;
                    player.Play();
                }
                else
                {
                    //"played new".Print();
                    player = new AudioStreamPlayer2D();
                    AddChild(player);
                    Players.Add(player);
                    player.Stream = Streams[sample];
                    player.Position = globalPosition;
                    player.Play();
                }
            }
        }

        /// <summary>
        /// Preloads audio assets
        /// </summary>
        private void Preload()
        {
            var samples = Enum.GetValues(typeof(Sample)).Cast<Sample>();
            foreach (var sample in samples)
            {
                Streams.Add(sample, GD.Load<AudioStream>(sample switch
                {
                    Sample.HORN => "res://Sound/game_Sounds_Alpha_Horn_Sound.mp3",
                    Sample.BB_Fire2 => "res://Sound/game_Sounds_Alpha_Horn_Sound.mp3",
                    _ => SFXPath + sample.ToString() + ".wav"
                }));

                //switch (sample)
                //{
                //    case Sample.HORN:

                //    case Sample.BB_Fire2: 
                //        continue;
                //    default:
                //        Streams.Add(sample, GD.Load<AudioStream>(SFXPath + sample.ToString() + ".wav"));
                //        break;
                //}
            }
        }
    }

    /// <summary>
    /// SFX audio sample
    /// </summary>
    public enum Sample
    {
        HORN,
        BB_Regen,
        BB_Build,
        BB_Hit,
        BB_Block,
        BB_Break,
        BB_Fire1,
        BB_Fire2,
    }
}
