using Godot;
using System;
using ldjam50;
using System.Collections.Generic;

public class DownToUpShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
        var shots = 1;
        boss.Fire(Vector2.Down);
        GD.Print("shooting");
        var baseRad = Mathf.Pi / 2;
        while (shots < 7)
        {
            //+ Mathf.Pi / 2 is Left. Divide that shit up
            var dir = new Vector2(Mathf.Cos(baseRad + (Mathf.Pi / 2)), Mathf.Sin(baseRad + (Mathf.Pi / 2)));
            Global.Time.AddNotify(shots * 0.1f, () => {
                boss.Fire(dir);
            });
            shots++;
        }
        Global.Time.AddNotify(shots * 0.1f, () => {
            boss.Fire(Vector2.Left);
            Done(this, new BossBehaviorDoneArgs(true));
        });
    }
}

