using Godot;
using System;
using ldjam50;
using System.Collections.Generic;
using ldjam50.TileBoss;

public class SpiralShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        var totalTime = 0.5f;
        var count = 25;
        var delay = totalTime / count;
        var callbacks = new List<Time.TimeNotifyCallback>();
        foreach (var dir in Math.LerpAngle(count, 0, Mathf.Pi * 2)) {
            callbacks.Add(() => boss.Fire(dir));
        }

        Global.Time.QueueNotify(delay, callbacks);
        Global.Time.AddNotify(totalTime, () => Done(this, new BossBehaviorDoneArgs()));
    }
}
