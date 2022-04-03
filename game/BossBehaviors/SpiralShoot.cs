using Godot;
using System;
using ldjam50;
using System.Collections.Generic;

public class SpiralShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    private Vector2[] directions = {
        Vector2.Left,
        new Vector2(-1, -1).Normalized(),
        Vector2.Up,
        new Vector2(1, -1).Normalized(),
        Vector2.Right,
        new Vector2(1, 1).Normalized(),
        Vector2.Down,
        new Vector2(-1, 1).Normalized()
    };

    public void Start(Boss boss) {
        var shots = 1;
        boss.Fire(directions[0]);
        while (shots < 7)
        {
            var dir = directions[shots];
            Global.Time.AddNotify(shots * 0.1f, () => {
                boss.Fire(dir);
            });
            shots++;
        }
        Global.Time.AddNotify(shots * 0.1f, () => {
            boss.Fire(directions[shots]);
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}
