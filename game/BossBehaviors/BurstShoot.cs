using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;
using ldjam50.Entities;

public class BurstShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => boss.Guns<BasicBossGun>().Count > 0;

    public void Start(TileBoss boss) {
        var guns = boss.Guns<BasicBossGun>();
        foreach (var gun in guns) gun.Shoot(Vector2.Left);

        Global.Time.AddNotify(0.15f, () => {
            var guns = boss.Guns<BasicBossGun>();
            foreach (var gun in guns) gun.Shoot(Vector2.Left);
        });

        Global.Time.AddNotify(0.3f, () => {
            var guns = boss.Guns<BasicBossGun>();
            foreach (var gun in guns) gun.Shoot(Vector2.Left);
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
