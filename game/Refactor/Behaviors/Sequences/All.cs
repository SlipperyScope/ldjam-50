using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Sequences
{
    /// <summary>
    /// Executes all sub behaviors
    /// <para>Pass: All behaviors pass
    /// <br />Fail: Any behavior fails
    /// <br />Abort: </para>
    /// </summary>
    public class All : Sequence
    {
        protected override void ExecuteSubBehavior()
        {
            if (Current == Behaviors.Count)
            {
                OnExecuteFinish(BehaviorStatus.Pass);
                return;
            }

            var behavior = Behaviors[Current];

            if (behavior.CanRun(Robot))
            {
                behavior.BehaviorFinished += SubBehaviorFinished;
                behavior.Execute(Robot);
            }
            else
            {
                OnExecuteFinish(BehaviorStatus.Fail);
            }
        }

        private void SubBehaviorFinished(IBehavior sender, IBehavior.BehaviorFinishedEventArgs e)
        {
            sender.BehaviorFinished -= SubBehaviorFinished;
            var status = sender.Mutate(Robot, e.Status);

            switch (status)
            {
                case BehaviorStatus.Pass:
                case BehaviorStatus.Ignore:
                    Current++;
                    ExecuteSubBehavior();
                    break;
                case BehaviorStatus.Fail:
                case BehaviorStatus.Abort:
                    OnExecuteFinish(status);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
