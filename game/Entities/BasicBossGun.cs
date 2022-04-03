using System;

namespace ldjam50.Entities
{
    public class BasicBossGun : BossGun
    {
        public override void Shoot()
        {
            throw new NotImplementedException();
        }

        public override void _Ready()
        {
            "Hi, am basic".Print();
        }
    }
}
