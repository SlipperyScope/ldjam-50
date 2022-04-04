using Godot;
using System;
using System.Collections.Generic;
using ldjam50;
using ldjam50.Entities;
using ldjam50.TileBoss;

public class PhaseEventArgs : EventArgs {
    public PhaseEventArgs(int phase) {
        Phase = phase;
    }
    public int Phase = 0;
}

public class DialogueLine {
    public Portrait Speaker;
    public string AudioPath;
    public string Line;
    public DialogueLine(Portrait speaker, string audioPath, string line) {
        Speaker = speaker;
        AudioPath = audioPath;
        Line = line;
    }
}

public class DialogueSequence {
    // Time to wait between audio clips
    public float Buffer = 0;
    public int CurrentLine = 0;

    public List<DialogueLine> Lines;
    public DialogueSequence(List<DialogueLine> lines, float buffer = 0) {
        Lines = lines;
        Buffer = buffer;
    }
}

public class Dialogue : Node
{
    [Export]
    protected NodePath HeroPath;
    [Export]
    protected NodePath BossPath;
    [Export]
    protected NodePath HeroPlayerPath;
    [Export]
    protected NodePath BossPlayerPath;
    Portrait Hero;
    Portrait Boss;
    HeroSprite HeroPlayer;
    TileBoss BossPlayer;
    AudioStreamPlayer Audio;

    public event EventHandler<PhaseEventArgs> PhaseShift;
    public event EventHandler<EventArgs> PhaseShiftComplete;

    public List<DialogueSequence> DialogConfig;

    // This is mostly for Godot
    DialogueSequence ActiveSequence;

    public override void _Ready()
    {
        Hero = GetNode<Portrait>(HeroPath);
        Boss = GetNode<Portrait>(BossPath);
        HeroPlayer = GetNode<Hero>(HeroPlayerPath).GetNode<HeroSprite>("Area2D");
        BossPlayer = GetNode<TileBoss>(BossPlayerPath);
        Audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

        // Make sure the HeroSprite suspends pew pew
        HeroPlayer.SetUsUpTheDialogue(this);

        // Hide things initially
        Hero.Visible = false;
        Boss.Visible = false;

        // Wire up dialogue triggering events
        HeroPlayer.Ded += DeathSequence;

        DialogConfig = new(){
            new(new(){
                new(Hero, "res://Sound/Dialogue/my_what_a_large_ship_you_have_compensating_much.mp3", "Hello, I am the hero of this space place"),
                new(Boss, "res://Sound/Dialogue/Your_foolishness_will_cost_you_your_life.mp3", "If you are a hero then explain your ship"),
                new(Hero, "res://Sound/Dialogue/i_can_do_this_all_day.mp3", "Well it's a couple guns and an, um, big bridge"),
                new(Boss, "res://Sound/Dialogue/this_only_ends_one_way.mp3", "Who made your bridge? Dyson, lmao"),
                // new(Hero, "", "Pathetic that you think that's insulting"),
            }),
            new(new() {
                new(Boss, "", "But your ship is mostly a sphere, how could it be?"),
                new(Hero, "", "Don't you see? I came to play ball"),
            }, 0.1f),
            new(new() {
                new(Hero, "", "Ha! I win again. Why don't you hit me with your best shot?"),
                new(Boss, "", "Oh ho ho ho, I'll fire away."),
            }, 0.1f),
            new(new() {
               new(Boss, "", "Nothing yet has mattered."),
               new(Hero, "", "Heh, so you're not keeping score?"),
               new(Boss, "", "You ignorant fool. Hubris will be your last mistake."),
            }),
            new(new() {
                new(Boss, "", "But that was my final form!!! You shouldn't even be able to see this line"),
                new(Hero, "", "But I am the alpha and omega. You cannot delay the inevitable"),
                new(Hero, "", "Muahahaahahahaha"),
            }, 0.1f),
        };

        BossPlayer.PhaseComplete += PhaseSequence;

        CallDeferred("StartEverything");
    }

    void StartEverything() {
        PhaseSequence(this, new PhaseCompleteEventArgs(){ Phase = 0 });
    }

    void PhaseSequence(object sender, PhaseCompleteEventArgs e) {
        // Determine dialogue sequence based on phase
        Play(DialogConfig[e.Phase]);

        // Tell the boss to start the next phase
        PhaseShift?.Invoke(this, new PhaseEventArgs(e.Phase));
    }

    void DeathSequence(object sender, EventArgs e) {
        // To be implemented, lmao
    }

    void Play(DialogueSequence sequence) {
        sequence.CurrentLine = 0;
        ActiveSequence = sequence;
        PlayNext();
    }

    void PlayNext() {
        float fakeTextLength = 3f;

        // Hide portraits, the speaker will unhide
        Hero.ExitStageLeft();
        Boss.ExitStageLeft();

        if (ActiveSequence.CurrentLine >= ActiveSequence.Lines.Count) {
            PhaseShiftComplete?.Invoke(this, new EventArgs());
            return;
        }

        var line = ActiveSequence.Lines[ActiveSequence.CurrentLine];
        line.Speaker.Say(line.Line);
        // Play the audio clip
        if (line.AudioPath != "") {
            var resource = GD.Load(line.AudioPath);
            Audio.Stream = resource as AudioStream;
            Audio.Connect("finished", this, nameof(PlayNext));
            Audio.Play();
        } else {
            Global.Time.AddNotify(fakeTextLength, () => PlayNext());
        }
        ActiveSequence.CurrentLine++;
    }
}
