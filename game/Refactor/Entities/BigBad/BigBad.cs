using Godot;
using ldjam50.Entities;
using ldjam50.Interfaces;
using ldjam50.Refactor.Behaviors;
using ldjam50.Refactor.Entities.BigBad.Templates;
using ldjam50.Refactor.Interfaces;
using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;

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

        public override void _EnterTree()
        {
            InitRobotVars();
        }

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            Controller = this.GetChild<BigBadController>();
            Hull = this.GetChild<BigBadHull>();
            GetNode<Sprite>("Core").Visible = false;

            //Global.Time.AddOneshot(2f, BuildTest);
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

        public void BuildShip(BigBadTemplate template)
        {
            Hull.HullEvent += HullEvent;
            Hull.Build(template);

        }

        private void HullEvent(System.Object sender, HullEventArgs e)
        {
            switch (e.HullAction)
            {
                case HullAction.StartBuild:
                    Vars.Write("Building", true);
                    break;
                case HullAction.CompleteBuild:
                    (sender as BigBadHull).HullEvent -= HullEvent;
                    Vars.Write("Building", false);
                    Vars.Write("HasHull", true);
                    break;
                default:
                    break;
            }
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

        #region IRobot
        public RobotVars Vars { get; private set; } = new();

        private void InitRobotVars()
        {
            Vars.WriteNew("HasHull", false);
            Vars.WriteNew("Building", false);
        }

        #endregion
    }
}