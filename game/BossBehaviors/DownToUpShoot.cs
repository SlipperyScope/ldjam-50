using Godot;
using System;
using ldjam50;
using System.Collections.Generic;
using ldjam50.TileBoss;
using ldjam50.Entities;

public class DownToUpShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => boss.Guns<SpiralBossGun>().Count > 0;

    public void Start(TileBoss boss) {
        var delay = 0.1f;
        var count = 8;
        var callbacks = new List<Time.NotifyCallback>();
        foreach (var dir in Math.LerpAngle(count, Mathf.Pi/2, Mathf.Pi)) {
            callbacks.Add(() => {
                Global.Time.QueueNotify(0.5f, new List<Time.NotifyCallback>(){
                    () => { foreach (var gun in boss.Guns<SpiralBossGun>()) gun.Shoot(dir); },
                    () => { foreach (var gun in boss.Guns<SpiralBossGun>()) gun.Shoot(dir); },
                    () => { foreach (var gun in boss.Guns<SpiralBossGun>()) gun.Shoot(dir); },
                });
            });
        }

        Global.Time.QueueNotify(delay, callbacks);
        Global.Time.AddNotify(delay * count + 0.5f * 3, () => Done(this, new BossBehaviorDoneArgs()));
    }
}

