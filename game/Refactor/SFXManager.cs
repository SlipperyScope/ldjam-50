using Godot;
using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        const Int32 TargetMaxPlayers = 15;

        /// <summary>
        /// Audio stream refs by ID
        /// </summary>
        private readonly Dictionary<Sample, AudioStream> Streams = new();

        /// <summary>
        /// Audio players
        /// </summary>
        private readonly List<SamplePlayer> Players = new();

        /// <summary>
        /// Enter Tree
        /// </summary>
        public override void _EnterTree()
        {
            for (var i = 0; i < TargetMaxPlayers; i++)
            {
                NewPlayer(true);
            }
        }

        /// <summary>
        /// Plays a sample at the center of the screen
        /// </summary>
        /// <param name="sample">Sample to play</param>
        public void Play(Sample sample) => Play(sample, (this.ScreenRect().Position + this.ScreenRect().End) / 2f);

        /// <summary>
        /// Plays a sample at a location
        /// </summary>
        /// <param name="sample">Sample to play</param>
        /// <param name="globalPosition">Location to play at in global coordinates</param>
        public void Play(Sample sample, Vector2 globalPosition)
        {
            if (sample is Sample.Null) throw new NullArgumentException($"Sample cannot be default value {nameof(Sample.Null)}");

            PlaybackData data;

            // TODO: Make sure all sfx are in
            if (PlaybackDatas.ContainsKey(sample) is false)
            {
                $"No data has been defined for {sample}. Attempting to create it automatically".Warn();
                data = new() { Stream = PlaybackData.LoadStream(sample) };
            }
            else
            {
                data = PlaybackDatas[sample];
            }

            SamplePlayer player = null;

            var playing = Players.Where(p => p.Playing && p.Stream == data.Stream);

            if (playing.Count() >= data.Voices)
            {
                if ((data.CutMethod & CutMethod.Newest) == CutMethod.Newest && playing.Any(p => p.GetPlaybackPosition() <= data.NewThreshold))
                {
                    player = Players.First();
                }
                else if ((data.CutMethod & CutMethod.Oldest) == CutMethod.Oldest && playing.Any(p => p.GetPlaybackPosition() >= data.OldThreshold))
                {
                    player = Players.Last();
                }
                else if ((data.CutMethod & CutMethod.None) == CutMethod.None)
                {
                    // Do not play
                    return;
                }
            }

            player ??= Players.FirstOrDefault(p => p.Playing is false && p.Stream == data.Stream) ?? Players.FirstOrDefault(p => p.Playing is false) ?? NewPlayer();
            player.Stream = data.Stream;
            player.VolumeDb = data.Volume;
            player.GlobalPosition = globalPosition;
            Players.Remove(player);
            Players.Insert(Players.Last().Playing is false ? Players.Count() - 1 : Players.FindIndex(p => p.Playing is true), player); // Keeps the list ordered newest to oldest per logic
            player.Play(0);
        }

        /// <summary>
        /// Gets the next available player
        /// </summary>
        /// <returns>Player</returns>
        private SamplePlayer NextReady() => Players.FirstOrDefault(p => p.Playing is false) ?? NewPlayer();

        /// <summary>
        /// Gets the next available player with the correct stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private SamplePlayer NextReady(AudioStream stream)
        {
            var ready = Players.Where(p => p.Playing is false);
            if (ready.Count() > 0)
            {
                return ready.FirstOrDefault(p => p.Stream == stream) ?? ready.FirstOrDefault();
            }
            else
            {
                return NewPlayer();
            }
        }

        /// <summary>
        /// Adds a new player to the pool and returns it
        /// </summary>
        /// <returns>New player</returns>
        private SamplePlayer NewPlayer(Boolean addToList = false)
        {
            var player = new SamplePlayer();
            if (addToList) Players.Insert(0, player);
            player.Connect("finished", this, nameof(PlayerFinished), new Godot.Collections.Array(player));
            AddChild(player);
            return player;
        }

        /// <summary>
        /// Returns player to available players
        /// </summary>
        /// <param name="player">Player to return</param>
        private void PlayerFinished(SamplePlayer player)
        {
            Players.Remove(player);

            if (Players.Count < TargetMaxPlayers || Players.First()?.Playing is true)
            {
                Players.Add(player);
            }
            else
            {
                player.Kill();
            }
        }

        /// <summary>
        /// Playback data
        /// </summary>
        public static readonly ReadOnlyDictionary<Sample, PlaybackData> PlaybackDatas = new(new Dictionary<Sample, PlaybackData>()
        {
            {
                Sample.HORN, new()
                {
                    Stream = PlaybackData.LoadStream("res://Sound/game_Sounds_Alpha_Horn_Sound.mp3", 1.5f),
                }
            },
            {
                Sample.BB_Build, new()
                {
                    Stream = PlaybackData.LoadStream(Sample.BB_Build, 1.25f),
                    Voices = 4,
                    CutMethod = CutMethod.Newest | CutMethod.Oldest | CutMethod.None,
                    NewThreshold = 0.01f,
                    OldThreshold = 0.2f,
                }
            },
            {
                Sample.BB_Regen, new()
                {
                    Stream = PlaybackData.LoadStream(Sample.BB_Regen),
                    Voices = 1,
                    CutMethod = CutMethod.Newest,
                    NewThreshold = 0f,
                }
            }
        });

        /// <summary>
        /// Player with theoretical optimizations to reading/writing Stream
        /// Idk if it's better and I'm too lazy to test
        /// </summary>
        private class SamplePlayer : AudioStreamPlayer2D
        {
            /// <summary>
            /// The AudioStream object to be played
            /// </summary>
            public new AudioStream Stream
            {
                get => _Stream;
                set
                {
                    if (value != _Stream)
                    _Stream = value;
                    base.Stream = value;
                }
            }
            private AudioStream _Stream;
        }
    }

    /// <summary>
    /// Data for playing back audio samples
    /// </summary>
    public record PlaybackData
    {
        /// <summary>
        /// Creates a new PlaybackData
        /// <para>Defaults:
        /// <br />Volume: 0
        /// <br />Bus: SFX
        /// <br />Voices: [no limit]
        /// <br />CutMethod: Newest | Oldest
        /// <br />NewThreshold: 0.01
        /// <br />OldThreshold: 0
        /// </para></summary>
        public PlaybackData() { }

        public const String SFXDefaultPath = "res://Refactor/Audio/SFX/";
        public const String SFXDefaultExtension = ".wav";

        /// <summary>
        /// Audio stream
        /// </summary>
        public AudioStream Stream { get; set; }

        /// <summary>
        /// Playback volume in dB relative to reference
        /// </summary>
        public Single Volume { get; set; } = 0f;

        /// <summary>
        /// Audio bus to use
        /// </summary>
        public AudioBus Bus { get; set; } = AudioBus.SFX;

        /// <summary>
        /// Number that can be simultaneously playing
        /// </summary>
        public Int32 Voices { get; set; } = Int32.MaxValue;

        /// <summary>
        /// How to cut after max instances
        /// <para>Newest: Cuts playback before NewThreshold 
        /// <br />Oldest: Buts playback after OldThreshold
        /// <br />None: Does not play, rather than cut
        /// <br />[either] | None: don't play if there's nothing in the threshold
        /// </para>
        /// </summary>
        /// <remarks>If no suitable cut is found the audio will not play</remarks>
        public CutMethod CutMethod { get; set; } = CutMethod.Newest | CutMethod.Oldest;

        /// <summary>
        /// Seconds into playback before sample is considered new
        /// </summary>
        public Single NewThreshold { get; set; } = 0.01f;

        /// <summary>
        /// Seconds into playback before sample is considered old
        /// </summary>
        public Single OldThreshold { get; set; } = 0f;

        /// <summary>
        /// Loads an audio stream
        /// </summary>
        /// <param name="fullPath">Path including file and extension</param>
        /// <returns></returns>
        public static AudioStream LoadStream(String fullPath) =>
            GD.Load<AudioStream>(fullPath);

        /// <summary>
        /// Loads an audio stream that varies pitch every playback
        /// </summary>
        /// <param name="fullPath">Path including file and extension</param>
        /// <param name="pitchVariation">Pitch variation (1 = no variation)</param>
        /// <returns></returns>
        public static AudioStream LoadStream(String fullPath, Single pitchVariation) =>
            new AudioStreamRandomPitch() { AudioStream = LoadStream(fullPath), RandomPitch = pitchVariation };

        /// <summary>
        /// Loads an audio stream
        /// </summary>
        /// <param name="sample">Sample name, excluding path and extension</param>
        /// <param name="path">Path where sample is located (default if null)</param>
        /// <param name="extension">Sample file extension (default if null)</param>
        /// <returns>Audio stream</returns>
        public static AudioStream LoadStream(String sample, String path = null, String extension = null) =>
            GD.Load<AudioStream>($"{path ?? SFXDefaultPath}{sample}{extension ?? SFXDefaultExtension}");

        /// <summary>
        /// Loads an audio stream
        /// </summary>
        /// <param name="sample">Sample</param>
        /// <param name="path">Path where sample is located (default if null)</param>
        /// <param name="extension">Sample file extension (default if null)</param>
        /// <returns>Audio stream</returns>
        public static AudioStream LoadStream(Sample sample, String path = null, String extension = null) =>
            LoadStream(sample.ToString(), path, extension);

        /// <summary>
        /// Loads an audio stream that varies pitch every playback
        /// </summary>
        /// <param name="sample">Sample name, excluding path and extension</param>
        /// <param name="pitchVariation">Pitch variation (1 = no variation)</param>
        /// <param name="path">Path where sample is located (default if null)</param>
        /// <param name="extension">Sample file extension (default if null)</param>
        /// <returns>Audio stream</returns>
        public static AudioStream LoadStream(String sample, Single pitchVariation, String path = null, String extension = null) =>
            new AudioStreamRandomPitch() { AudioStream = LoadStream(sample, path, extension), RandomPitch = pitchVariation };

        /// <summary>
        /// Loads an audio stream that varies pitch every playback
        /// </summary>
        /// <param name="sample">Sample</param>
        /// <param name="pitchVariation">Pitch variation (1 = no variation)</param>
        /// <param name="path">Path where sample is located (default if null)</param>
        /// <param name="extension">Sample file extension (default if null)</param>
        /// <returns>Audio stream</returns>
        public static AudioStream LoadStream(Sample sample, Single pitchVariation, String path = null, String extension = null) =>
            LoadStream(sample.ToString(), pitchVariation, path, extension);
    }

    /// <summary>
    /// Cut method
    /// </summary>
    [Flags]
    public enum CutMethod
    {
        Newest = 0b001,
        Oldest = 0b010,
        None   = 0b100,
    }

    /// <summary>
    /// SFX audio sample
    /// </summary>
    public enum Sample
    {
        Null,
        HORN,
        BB_Regen,
        BB_Build,
        BB_Hit,
        BB_Block,
        BB_Break,
        BB_Fire1,
        BB_Fire2,
    }

    /// <summary>
    /// Audio bus
    /// </summary>
    public enum AudioBus
    {
        Master,
        SFX
    }
}
