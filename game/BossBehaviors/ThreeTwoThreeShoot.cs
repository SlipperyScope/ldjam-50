using Godot;
using System;
using ldjam50;

public class ThreeTwoThreeShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
        boss.Fire(Vector2.Left);
        boss.Fire(new Vector2(-1, -0.3f));
        boss.Fire(new Vector2(-1, 0.3f));

        Global.Time.AddNotify(0.15f, () => {
            boss.Fire(new Vector2(-1, -0.15f));
            boss.Fire(new Vector2(-1, 0.15f));
        });

        Global.Time.AddNotify(0.3f, () => {
            boss.Fire(Vector2.Left);
            boss.Fire(new Vector2(-1, -0.3f));
            boss.Fire(new Vector2(-1, 0.3f));
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
