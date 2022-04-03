using Godot;
using System;
using ldjam50;

public class BoringShoot : Node, IBossBehavior
{
    public event EventHandler<BossBehaviorDoneArgs> Done;
    // New
    // public bool IsAvailable(Boss boss) => boss.Guns<BasicBossGun>().Count > 0;
    public bool IsAvailable(Boss boss) => true;

    public void Start(Boss boss) {
        // New
        // var guns = boss.Guns<BasicBossGun>();
        // foreach (var gun in guns) {
        //     gun.Fire(Vector2.Left);
        // }

        // Old
        boss.Fire(Vector2.Left);
        Done(this, new BossBehaviorDoneArgs(true));
    }
}
