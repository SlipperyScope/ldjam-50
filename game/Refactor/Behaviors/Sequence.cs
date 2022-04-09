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
    public class Sequence : Behavior
    {
        private List<Behavior> Behaviors;
        private Int32 Current = 0;
        private IRobot Robot;

        public override void _Ready()
        {
            var children = GetChildren().ToList();
            if (children.Any(c => c is not Behavior)) throw new InvalidChildException($"Children are not {typeof(Behavior)}");
            Behaviors = children.Select(c => c as Behavior).ToList();
        }

        public override void Execute(IRobot robot)
        {
            Robot = robot;
            ExecuteSubbehavior();
        }

        private void ExecuteSubbehavior()
        {
            if (Behaviors.Count == 0)
            {
                Finish(true);
            }
            else if (Current >= Behaviors.Count)
            {
                Current = 0;
                Finish(true);
            }
            else
            {
                var behavior = Behaviors[Current];
                behavior.ExecuteFinish += SubBehaviorFinish;
                behavior.Execute(Robot);
            }
        }

        private void SubBehaviorFinish(Object sender, ExecuteFinishEventArgs e)
        {
            var behavior = sender as Behavior;
            behavior.ExecuteFinish -= SubBehaviorFinish;

            switch (e.Status)
            {
                case ExecuteStatus.Success:
                    Current++;
                    ExecuteSubbehavior();
                    break;
                case ExecuteStatus.Failure:
                    Current = 0;
                    Finish(false);
                    break;
                case ExecuteStatus.Abort:
                    Current = 0;
                    Abort(behavior);
                    break;
                default:
                    break;
            }
        }
    }
}