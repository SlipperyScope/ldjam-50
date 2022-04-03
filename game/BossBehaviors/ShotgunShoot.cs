using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;

public class ShotgunShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        // TODO: Replace random angle with player angle to player position
        float ang = (float)GD.RandRange(Mathf.Pi / 2, Mathf.Pi / 2 * 3);
        var count = 10;

        foreach (var dir in Math.FanAngle(count, ang, Mathf.Pi/4)) {
            boss.Fire(dir);
        }
        Done(this, new BossBehaviorDoneArgs());
    }
}
