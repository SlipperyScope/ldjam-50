using Godot;
using System;
using ldjam50;

public class BoringShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;

    public void Start(Boss boss) {
        GD.Print("Shoot started");

        boss.Fire();
        Done(this, new BossBehaviorDoneArgs(true));
    }
}
