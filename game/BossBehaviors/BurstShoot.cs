using Godot;
using System;
using ldjam50;

public class BurstShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
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
