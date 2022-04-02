using Godot;
using System;

public class BossBehaviorDoneArgs : EventArgs {
    // Placeholder, may be useful, idk, stop asking questions
    public BossBehaviorDoneArgs(bool terminal = false) {
        Terminal = terminal;
    }
    public bool Terminal = false;
}

public interface IBossBehavior {
    public void Start();
    public event EventHandler<BossBehaviorDoneArgs> Done;
}

public class BossManager : Node
{
    public IBossBehavior StartingBehavior;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Starting manager");
        StartingBehavior = new Wait();
        StartingBehavior.Done += Next;
        StartingBehavior.Start();
    }

    void Next(object sender, BossBehaviorDoneArgs e) {
        GD.Print("In the next method wow ", e.Terminal);
    }
}
