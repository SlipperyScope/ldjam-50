using Godot;
using System;
using ldjam50;
using System.Collections.Generic;
using ldjam50.TileBoss;
using ldjam50.Entities;

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
                var guns = boss.Guns<SpiralBossGun>();
                foreach (var gun in guns) {
                    Global.Time.QueueNotify(0.5f, new List<Time.TimeNotifyCallback>(){
                        () => gun.Shoot(dir),
                        () => gun.Shoot(dir),
                        () => gun.Shoot(dir),
                    });
                }
            });
        }

        Global.Time.QueueNotify(delay, callbacks);
        Global.Time.AddNotify(delay * count + 0.5f * 3, () => Done(this, new BossBehaviorDoneArgs()));
    }
}

