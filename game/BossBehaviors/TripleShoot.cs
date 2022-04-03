using Godot;
using System;
using ldjam50;
using ldjam50.TileBoss;

public class TripleShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    public bool IsAvailable(TileBoss boss) => true;

    public void Start(TileBoss boss) {
        boss.Fire(Vector2.Left);
        boss.Fire(new Vector2(-1, -0.3f));
        boss.Fire(new Vector2(-1, 0.3f));
        Done(this, new BossBehaviorDoneArgs(true));
    }
}
