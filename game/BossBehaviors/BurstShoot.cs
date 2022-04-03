using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;

public class BurstShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        boss.Fire(Vector2.Left);

        Global.Time.AddNotify(0.15f, () => {
            boss.Fire(Vector2.Left);
        });

        Global.Time.AddNotify(0.3f, () => {
            boss.Fire(Vector2.Left);
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
