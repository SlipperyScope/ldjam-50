using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    public enum ExecuteStatus
    {
        Success,
        Failure,
        Abort,
    }

    public delegate void ExecuteFinishEventHandler(object sender, ExecuteFinishEventArgs e);

    public class ExecuteFinishEventArgs : EventArgs
    {
        public readonly ExecuteStatus Status;

        public ExecuteFinishEventArgs(ExecuteStatus status)
        {
            Status = status;
        }
    }
}
