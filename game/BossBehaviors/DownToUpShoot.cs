using Godot;
using System;
using ldjam50;
using System.Collections.Generic;

public class DownToUpShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
        var count = 8;
        var delay = 0.1f;
        for (int i = 0; i < count; i++) {
            var ang =  Mathf.Pi / 2 / (count - 1) * i;
            var dir = Vector2.Down.Rotated(ang);
            Global.Time.AddNotify(i * delay + delay, () => {
                boss.Fire(dir);
            });
        }

        Global.Time.AddNotify(count * delay + delay, () => {
            Done(this, new BossBehaviorDoneArgs());
        });
    }
}

