using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;

public class Wait : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        Global.Time.AddNotify(1.0f, () => {
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
