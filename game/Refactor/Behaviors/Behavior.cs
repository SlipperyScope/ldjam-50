using Godot;
using ldjam50.Refactor.Interfaces;
using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    /// <summary>
    /// Base class for behaviors
    /// </summary>
    /// <remarks>Finish() or Abort() must be called after 
    /// execute for the behavior to complete</remarks>
    public abstract class Behavior : Node, IBehavior
    {
        public event IBehavior.BehaviorFinishedEventHandler BehaviorFinished;

        public List<ICondition> Conditions { get; private set; } = new();
        public List<IMutator> Mutators { get; private set; } = new();

        public Boolean CanRun(IRobot robot) => Conditions.All(c => c.CanRun(robot));
        public BehaviorStatus Mutate(IRobot robot, BehaviorStatus status)
        {
            Mutators.ForEach(m => status = m.Mutate(robot, status));
            return status;
        }

        [Export]
        protected Boolean Debug = false;

        /// <summary>
        /// Ready
        /// </summary>
        /// <exception cref="InvalidChildException"></exception>
        public override void _Ready()
        {
            var children = GetChildren().ToList();

            foreach(var child in children)
            {
                switch(child)
                {
                    case ICondition condition: Conditions.Add(condition); break;
                    case IMutator mutator: Mutators.Add(mutator); break;
                    default:
                        throw new InvalidChildException($"{GetPath()} contains unsupported children");
                }
            }
        }

        /// <summary>
        /// Executes a behavior
        /// </summary>
        public abstract void Execute(IRobot robot);

        /// <summary>
        /// Finishes execution
        /// </summary>
        protected virtual void Finish(Boolean success) => OnBehaviorFinished(success ? BehaviorStatus.Pass : BehaviorStatus.Fail);

        /// <summary>
        /// Aborts execution
        /// </summary>
        /// <param name="source">Source of the abortion</param>
        protected virtual void Abort() => OnBehaviorFinished(BehaviorStatus.Abort);

        /// <summary>
        /// Dispatches behavior finished event
        /// </summary>
        /// <param name="status">Status</param>
        protected void OnBehaviorFinished(BehaviorStatus status)
        {
            if (Debug) $"{GetPath()} finished with status {status}".Print();
            BehaviorFinished(this, new IBehavior.BehaviorFinishedEventArgs(status));
        }
    }
}
