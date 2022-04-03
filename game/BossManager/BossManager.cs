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
    public BehaviorMapping[] Next;
    public BehaviorMapping[] Interrupt;
    public BehaviorMapping(string name, BehaviorMapping[] next, BehaviorMapping[] interrupt) {
        Name = name;
        Next = next;
        Interrupt = interrupt;
    }
    public BehaviorMapping(string name, params BehaviorMapping[] next) {
        Name = name;
        Next = next;
    }
    public BehaviorMapping Any(BehaviorMapping[] behaviors) {
        var idx = GD.Randi() % behaviors.Length;
        GD.Print($"Got index {idx} of {behaviors.Length}");
        var behavior = behaviors[idx];
        GD.Print($"Got behavior {behavior}");
        return behavior;
        // return behaviors[GD.Randi() % behaviors.Length];
    }
    public BehaviorMapping AnyNext() {
        return Any(Next);
    }

    public BehaviorMapping AnyInterrupt() {
        if (Interrupt is not null) return Any(Interrupt);
        return null;
    }

    public override string ToString() {
        return $"BehaviorMapping ({Name})";
    }
}

public class BossPhase1Config : IBossBehaviorConfig {
    public BehaviorMapping Initial {
        get { GD.Print("yeah okay..."); GD.Print(BossPhase1Config.Wait); return BossPhase1Config.Wait; }
    }
    public static BehaviorMapping Shoot = new BehaviorMapping("BoringShoot", BossPhase1Config.Wait);
    public static BehaviorMapping Wait = new BehaviorMapping(
        "Wait",
        new BehaviorMapping[]{BossPhase1Config.Shoot},
        new BehaviorMapping[]{BossPhase1Config.Shoot}
    );
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
        GD.Print("pls");
        GD.Print(ActiveBehavior);
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
