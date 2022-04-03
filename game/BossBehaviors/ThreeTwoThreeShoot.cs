using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;
using ldjam50.Entities;

public class ThreeTwoThreeShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {

        var guns = boss.Guns<BasicBossGun>();
        foreach (var gun in guns) {
            gun.Shoot(Vector2.Left);
            gun.Shoot(new Vector2(-1, -0.3f));
            gun.Shoot(new Vector2(-1, 0.3f));
        }

        Global.Time.AddNotify(0.15f, () => {
            var guns = boss.Guns<BasicBossGun>();
            foreach (var gun in guns) {
                gun.Shoot(new Vector2(-1, -0.15f));
                gun.Shoot(new Vector2(-1, 0.15f));
            }
        });

        Global.Time.AddNotify(0.3f, () => {
            var guns = boss.Guns<BasicBossGun>();
            foreach (var gun in guns) {
                gun.Shoot(Vector2.Left);
                gun.Shoot(new Vector2(-1, -0.3f));
                gun.Shoot(new Vector2(-1, 0.3f));
            }
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
