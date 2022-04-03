using Godot;
using System;
using System.Collections.Generic;

public class BossBehaviorDoneArgs : EventArgs {
    // Placeholder, may be useful, idk, stop asking questions
    public BossBehaviorDoneArgs(bool terminal = false) {
        Terminal = terminal;
    }
    public bool Terminal = false;
}

public interface IBossBehavior {
    public void Start(Boss boss);
    public event EventHandler<BossBehaviorDoneArgs> Done;
}

public interface IBossBehaviorConfig {
    public BehaviorMapping Initial { get; }
}

public static class BossBehaviors {
    public delegate IBossBehavior Builder();

    static Dictionary<string,Builder> Map = new Dictionary<string, Builder>(){
        { "Wait", () => new Wait() },
        { "BoringShoot", () => new BoringShoot() }
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
        int idx = (int)GD.Randi() % behaviors.Count;
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
}

public class BossPhase1Config : IBossBehaviorConfig {
    BehaviorMapping _Initial;
    public BehaviorMapping Initial {
        get { return _Initial; }
    }

    public BossPhase1Config() {
        var wait = new BehaviorMapping("Wait");
        var shoot = new BehaviorMapping("BoringShoot", wait);

        // "Bootstrap" the config by setting edges in initial node
        wait.Next.Add(shoot);
        _Initial = wait;
    }
}

public class BossManager : Node {
    public Boss DaBoss;
    public IBossBehaviorConfig Config = new BossPhase1Config();
    public BehaviorMapping ActiveBehavior;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DaBoss = GetTree().Root.GetNode<Boss>("Game/Boss");
        GD.Print("Starting manager");

        ActiveBehavior = Config.Initial;
        Run();
    }

    void Run() {
        GD.Print($"Running a '{ActiveBehavior.Name}'");
        var behavior = BossBehaviors.MakeA(ActiveBehavior.Name);
        behavior.Done += Next;
        behavior.Start(DaBoss);
    }

    void Next(object sender, BossBehaviorDoneArgs e) {
        GD.Print("In the next method wow ", e.Terminal);

        ActiveBehavior = ActiveBehavior.AnyNext();
        Run();
    }
}
