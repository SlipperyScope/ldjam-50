using Godot;
using System;
using ldjam50.TileBoss;
using ldjam50;

public class Move : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    // New
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        float angle = GD.Randf() * 2 * Mathf.Pi;
        boss.MoveDirection = new Vector2((float)Mathf.Sin(angle), (float)Mathf.Cos(angle));
        

        Global.Time.AddNotify(GD.Randf() * 5, () => {
            boss.MoveDirection = new Vector2(0, 0);
        });
        Done(this, new BossBehaviorDoneArgs(true));
    }
}
