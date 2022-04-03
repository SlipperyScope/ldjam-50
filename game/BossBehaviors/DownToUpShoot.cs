using Godot;
using System;
using ldjam50;
using System.Collections.Generic;
using ldjam50.TileBoss;

public class DownToUpShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        var delay = 0.1f;
        var count = 8;
        var callbacks = new List<Time.TimeNotifyCallback>();
        foreach (var dir in Math.LerpAngle(count, Mathf.Pi/2, Mathf.Pi)) {
            callbacks.Add(() => {
                Global.Time.QueueNotify(0.5f, new List<Time.TimeNotifyCallback>(){
                    () => boss.Fire(dir),
                    () => boss.Fire(dir),
                    () => boss.Fire(dir),
                });
            });
        }

        Global.Time.QueueNotify(delay, callbacks);
        Global.Time.AddNotify(delay * count + 0.5f * 3, () => Done(this, new BossBehaviorDoneArgs()));
    }
}

