using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Entities
{
    public abstract class BossGun : Node2D
    {
        public abstract void Shoot(Vector2 dir);
    }
}
