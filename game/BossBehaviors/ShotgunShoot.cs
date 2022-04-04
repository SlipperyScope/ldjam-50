using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;
using ldjam50.Entities;

public class ShotgunShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => boss.Guns<BasicBossGun>().Count > 0;

    public void Start(TileBoss boss) {
        var count = 10;

        var guns = boss.Guns<BasicBossGun>();
        var gun = guns[(int)(GD.Randi() % guns.Count)];
        // TODO: Replace random angle with player angle to player position
        float ang = (float)GD.RandRange(Mathf.Pi / 2, Mathf.Pi / 2 * 3);
        foreach (var dir in Math.FanAngle(count, ang, Mathf.Pi/4)) {
            gun.Shoot(dir);
        }


        Done(this, new BossBehaviorDoneArgs());
    }
}
