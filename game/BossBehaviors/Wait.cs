using Godot;
using System;
using ldjam50;

public class Wait : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
        GD.Print("Wait started");
        
        Global.Time.AddNotify(1.0f, () => {
            GD.Print("Time expired");
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
