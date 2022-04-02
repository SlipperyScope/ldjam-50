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
    public IBossBehavior startingBehavior;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Start the machine here. 
    }
}
