using Godot;
using System;
using ldjam50;

public class BoringShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
        boss.Fire(Vector2.Left);
        Done(this, new BossBehaviorDoneArgs(true));
    }
}
