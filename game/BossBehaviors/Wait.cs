using Godot;
using System;
using ldjam50;

public class Wait : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start() {
        GD.Print("Behavior started");
        
        Global.Time.AddNotify(5.0f, () => {
            GD.Print("Time expired");
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
