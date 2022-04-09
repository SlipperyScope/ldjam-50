using Godot;
using ldjam50.Entities;
using ldjam50.Interfaces;
using ldjam50.Refactor.Entities.BigBad.Templates;
using ldjam50.Refactor.Interfaces;
using ldjam50.Refactor.Utils;
using System;

namespace ldjam50.Refactor.Entities.BigBad
{
    /// <summary>
    /// Big bad boss
    /// </summary>
    public class BigBad : KinematicBody2D, IDamageable, IRobot
    {
        private BigBadController Controller;
        private BigBadHull Hull;

        /// <summary>
        /// Velocity in meters per second
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Controller = this.GetChild<BigBadController>();
            Hull = this.GetChild<BigBadHull>();
            GetNode<Sprite>("Core").Visible = false;

            Global.Time.AddOneshot(2f, BuildTest);
        }

        /// <summary>
        /// Builds the test hull
        /// </summary>
        private void BuildTest()
        {
            var template = GD.Load<PackedScene>(
                "res://Refactor/Entities/BigBad/Templates/Template_Test.tscn").Instance<BigBadTemplate>();
            BuildShip(template);
        }

        /// <summary>
        /// Physics Process
        /// </summary>
        public override void _PhysicsProcess(Single delta)
        {
            Position += Velocity * Global.PxPM * delta;
        }

        #region IDamageable
        public void ApplyDamage(DamageInfo info)
        {
            if (info is ImpactDamageInfo impact)
            {
                Velocity += impact.Direction * Global.PxPM * impact.Damage;
            }
        }
        #endregion

        public void BuildShip(BigBadTemplate template)
        {
            Hull.Build(template);
        }
    }
}