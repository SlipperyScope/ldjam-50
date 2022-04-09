using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Sequences
{
    /// <summary>
    /// Executes sub behaviors sequentially until one passes
    /// <para>Pass: Any behavior passes
    /// <br />Fail: All behaviors fail
    /// <br />Abort: Any behavior aborts</para>
    /// </summary>
    public class Any : Sequence
    {
        protected override void ExecuteSubBehavior()
        {
            IBehavior behavior;

            do
            {
                if (Current == Behaviors.Count)
                {
                    OnExecuteFinish(BehaviorStatus.Fail);
                    return;
                }

                behavior = Behaviors[Current++];
            } while (behavior.CanRun(Robot) is false);

            Current--;

            behavior.BehaviorFinished += SubBehaviorFinished;
            behavior.Execute(Robot);
        }

        private void SubBehaviorFinished(IBehavior sender, IBehavior.BehaviorFinishedEventArgs e)
        {
            sender.BehaviorFinished -= SubBehaviorFinished;
            var status = sender.Mutate(Robot, e.Status);

            switch (status)
            {
                case BehaviorStatus.Pass:
                case BehaviorStatus.Ignore:
                    OnExecuteFinish(BehaviorStatus.Pass);
                    break;
                case BehaviorStatus.Fail:
                    Current++;
                    ExecuteSubBehavior();
                    break;
                case BehaviorStatus.Abort:
                    OnExecuteFinish(BehaviorStatus.Abort);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
