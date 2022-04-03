using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Entities
{
    public abstract class BossGun : Node2D
    {
        public abstract void Shoot();
    }

    public class BasicBossGun : BossGun
    {
        public override void Shoot()
        {
            throw new NotImplementedException();
        }
    }

    public class SpiralBossGun : BossGun
    {
        public override void Shoot()
        {
            throw new NotImplementedException();
        }
    }
}
