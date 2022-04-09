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
    /// Executes it's child behavior only if a condition is true
    /// </summary>
    public abstract class Condition : Behavior
    {
        private Behavior Behavior;

        public override void _Ready()
        {
            if (GetChildCount() > 1) throw new InvalidChildException("Root behavior can only have one child");
            if (GetChild(0) is not Behavior behavior)
            {
                throw new InvalidNodeCastException("Child is not a behavior");
            }
            Behavior = behavior;
            Behavior.ExecuteFinish += ChildBehaviorFinish;
        }

        public sealed override void Execute(IRobot robot)
        {
            if (CanRun(robot))
            {
                Behavior.Execute(robot);
            }
            else
            {
                Finish(false);
            }
        }

        private void ChildBehaviorFinish(Object sender, ExecuteFinishEventArgs e)
        {
            switch (e.Status)
            {
                case ExecuteStatus.Success:
                    Finish(true);
                    break;
                case ExecuteStatus.Failure:
                    Finish(false);
                    break;
                case ExecuteStatus.Abort:
                    Abort();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Checks if the child behavior can run
        /// </summary>
        /// <returns>True if the child behavior can run</returns>
        public abstract Boolean CanRun(IRobot robot);

        public override void _ExitTree()
        {
            Behavior.ExecuteFinish -= ChildBehaviorFinish;
        }
    }
}
