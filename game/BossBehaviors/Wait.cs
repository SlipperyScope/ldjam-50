using Godot;
using System;
using ldjam50;

public class Wait : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(Boss boss) => true;

    public void Start(Boss boss) {
        Global.Time.AddNotify(1.0f, () => {
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
