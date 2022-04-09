using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Interfaces
{
    public interface IDamageable
    {
        /// <summary>
        /// Applies damage
        /// </summary>
        /// <param name="info">Info about the damage</param>
        public void ApplyDamage(DamageInfo info);
    }

    /// <summary>
    /// Info about damage
    /// </summary>
    public record DamageInfo
    {
        /// <summary>
        /// Damage amount
        /// </summary>
        public Single Damage { get; private set; }

        public DamageInfo(Single damage)
        {
            Damage = damage;
        }
    }

    /// <summary>
    /// Info about impact damage
    /// </summary>
    public record ImpactDamageInfo : DamageInfo
    {
        /// <summary>
        /// Position of impact
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Direction of impact
        /// </summary>
        public Vector2 Direction { get; private set; }

        public ImpactDamageInfo(Single damage, Vector2 position, Vector2 direction) : base(damage)
        {
            Position = position;
            Direction = direction;
        }
    }
}
