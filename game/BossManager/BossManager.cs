using Godot;
using ldjam50;
using ldjam50.TileBoss;
using System;
using System.Collections.Generic;
using System.Linq;

public class BossBehaviorDoneArgs : EventArgs {
    // Placeholder, may be useful, idk, stop asking questions
    public BossBehaviorDoneArgs(bool terminal = false) {
        Terminal = terminal;
    }
    public bool Terminal = false;
}

public interface IBossBehavior {
    public void Start(TileBoss boss);
    public bool IsAvailable(TileBoss boss);
    public event EventHandler<BossBehaviorDoneArgs> Done;
}

public interface IBossBehaviorConfig {
    public BehaviorMapping Initial { get; }
}

public static class BossBehaviors {
    public delegate IBossBehavior Builder();

    static Dictionary<string,Builder> Map = new Dictionary<string, Builder>(){
        { "Wait", () => new Wait() },
        { "Move", () => new Move() },
        { "Boring", () => new BoringShoot() },
        { "Burst", () => new BurstShoot() },
        { "DownToUp", () => new DownToUpShoot() },
        { "Spiral", () => new SpiralShoot() },
        { "ThreeTwoThree", () => new ThreeTwoThreeShoot() },
        { "Triple", () => new TripleShoot() },
        { "Shotgun", () => new ShotgunShoot() },
    };
    public static IBossBehavior MakeA(string name) {
        if (!Map.ContainsKey(name)) throw new Exception($"Steve pls, there's no bulder for the name '{name}'.");
        return Map[name]();
    }
}

public class BehaviorMapping {
    public string Name;
    public List<BehaviorMapping> Next;
    public List<BehaviorMapping> Interrupt;
    public BehaviorMapping(string name) {
        Name = name;
        Next = new List<BehaviorMapping>();
        Interrupt = new List<BehaviorMapping>();
    }
    public BehaviorMapping(string name, params BehaviorMapping[] next) {
        Name = name;
        Next = new List<BehaviorMapping>(next);
        Interrupt = new List<BehaviorMapping>();
    }
    public BehaviorMapping Any(List<BehaviorMapping> behaviors) {
        if (behaviors.Count == 0) return null;
        int idx = (int)(GD.Randi() % behaviors.Count);
        return behaviors[idx];
    }
    public BehaviorMapping AnyNext() {
        return Any(Next);
    }

    public BehaviorMapping AnyInterrupt() {
        return null;
    }

    public override string ToString() {
        return $"BehaviorMapping ({Name})";
    }

    public void NextEdges(params BehaviorMapping[] next) {
        Next = new List<BehaviorMapping>(next);
    }
}

public class BossPhase1Config : IBossBehaviorConfig {
    BehaviorMapping _Initial;
    public BehaviorMapping Initial {
        get { return _Initial; }
    }

    public BossPhase1Config() {
        var wait = new BehaviorMapping("Wait");
        var move = new BehaviorMapping("Move");
        var boring = new BehaviorMapping("Boring", wait);
        var burst = new BehaviorMapping("Burst", wait);
        var downToUp = new BehaviorMapping("DownToUp", wait);
        var spiral = new BehaviorMapping("Spiral", boring);
        var threeTwoThree = new BehaviorMapping("ThreeTwoThree", wait);
        var triple = new BehaviorMapping("Triple", wait);
        var shotgun = new BehaviorMapping("Shotgun", wait);

        // "Bootstrap" the config by setting edges in initial node
        wait.NextEdges(boring, burst, downToUp, spiral, threeTwoThree, triple, shotgun, move, move, move, move, move);
        move.NextEdges(wait, boring, burst, downToUp, spiral, threeTwoThree, triple, shotgun);
        _Initial = wait;
    }
}

public class BossManager : Node {
    public TileBoss DaBoss;
    public IBossBehaviorConfig Config = new BossPhase1Config();
    public BehaviorMapping ActiveBehavior;
    public BehaviorMapping QueuedBehavior;
    public int Iteration = 0;
    bool IsPlaying = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DaBoss = this.GetParent<TileBoss>();

        ActiveBehavior = Config.Initial;
        DaBoss.Dialogue.PhaseShift += (_, _) => Pause();
        DaBoss.Dialogue.PhaseShiftComplete += (_, _) => Play();
        Global.Time.AddNotify(0f, Run);
    }

    void Play() {
        this.IsPlaying = true;
        if (QueuedBehavior is not null) {
            ActiveBehavior = QueuedBehavior;
            QueuedBehavior = null;
            Run();
        }
    }

    void Pause() {
        IsPlaying = false;
    }

    void Run() {
        var behavior = BossBehaviors.MakeA(ActiveBehavior.Name);
        behavior.Done += Next;
        behavior.Start(DaBoss);
    }

    void Next(object sender, BossBehaviorDoneArgs e) {
        var availableBehaviors = ActiveBehavior.Next.Where(m => {
            var b = BossBehaviors.MakeA(m.Name);
            return b.IsAvailable(DaBoss);
        }).ToList();

        // If no behavior is available (which is always true while a boss is 'loading' in)
        // fallback to the initial state (which is probably Wait)
        ActiveBehavior = ActiveBehavior.Any(availableBehaviors) ?? Config.Initial;

        // if (ActiveBehavior == Config.Initial) Iteration++;
        // ActiveBehavior = ActiveBehavior.Next[Iteration % ActiveBehavior.Next.Count];

        if (IsPlaying) {
            Run();
        } else {
            QueuedBehavior = ActiveBehavior;
        }
    }
}