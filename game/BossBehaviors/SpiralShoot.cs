using Godot;
using System;
using ldjam50;
using System.Collections.Generic;

public class SpiralShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
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
