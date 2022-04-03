using Godot;
using System;
using ldjam50;
using System.Collections.Generic;
using ldjam50.TileBoss;
using ldjam50.Entities;

public class SpiralShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => boss.Guns<SpiralBossGun>().Count > 0;

    public void Start(TileBoss boss) {
        var totalTime = 0.5f;
        var count = 25;
        var delay = totalTime / count;
        var callbacks = new List<Time.TimeNotifyCallback>();
        foreach (var dir in Math.LerpAngle(count, 0, Mathf.Pi * 2)) {
            callbacks.Add(() => {
                var guns = boss.Guns<SpiralBossGun>();
                foreach (var gun in guns) {
                    gun.Shoot(dir);
                }
            });
        }

        Global.Time.QueueNotify(delay, callbacks);
        Global.Time.AddNotify(totalTime, () => Done(this, new BossBehaviorDoneArgs()));
    }
}
