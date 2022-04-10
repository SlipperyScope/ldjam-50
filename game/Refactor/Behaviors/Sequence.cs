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
    /// Sequentially executues child behaviors
    /// <para>
    /// <br />Success: All behaviors execute successfully
    /// <br />Failure: On failure of any behavior
    /// <br />Abort:   On abort of any behavior
    /// </para></summary>
    public abstract class Sequence : Node, IBehavior
    {
        public event IBehavior.BehaviorFinishedEventHandler BehaviorFinished;

        protected List<IBehavior> Behaviors = new();
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

        protected Int32 Current = 0;
        protected IRobot Robot;

        /// <summary>
        /// Ready
        /// </summary>
        /// <exception cref="InvalidChildException">Non-behavior child</exception>
        public override void _Ready()
        {
            var children = GetChildren().ToList();
            if (children.All(c => c is IBehavior or ICondition or IMutator) is false) throw new InvalidChildException($"{GetPath()} has invalid children");
            Behaviors = children.Where(c => c is IBehavior).Select(b => b as IBehavior).ToList();
            Conditions = children.Where(c => c is ICondition).Select(c => c as ICondition).ToList();
            Mutators = children.Where(c => c is IMutator).Select(m => m as IMutator).ToList();
        }

        public void Execute(IRobot robot)
        {
            if (Debug) "Executing".Print(GetPath());

            if (Behaviors.Count == 0)
            {
                OnExecuteFinish(BehaviorStatus.Ignore);
                return;
            }

            Robot = robot;
            ExecuteSubBehavior();
        }

        /// <summary>
        /// Executes a subbehavior
        /// </summary>
        protected abstract void ExecuteSubBehavior();

        protected virtual void OnExecuteFinish(BehaviorStatus status)
        {
            Current = 0;
            if (Debug) status.Print($"{GetPath()} finished with status");
            BehaviorFinished?.Invoke(this, new IBehavior.BehaviorFinishedEventArgs(status));
        }
    }
}