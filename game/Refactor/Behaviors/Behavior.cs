using Godot;
using ldjam50.Refactor.Interfaces;
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
    public abstract class Behavior : Node
    {
        public event ExecuteFinishEventHandler ExecuteFinish;

        /// <summary>
        /// Executes a behavior
        /// </summary>
        public abstract void Execute(IRobot robot);

        /// <summary>
        /// Finishes execution
        /// </summary>
        protected void Finish(Boolean success)
        {
            ExecuteFinish?.Invoke(this, new ExecuteFinishEventArgs(success ? ExecuteStatus.Success : ExecuteStatus.Failure));
        }

        /// <summary>
        /// Aborts execution
        /// </summary>
        /// <param name="source">Source of the abortion</param>
        protected void Abort(Behavior source)
        {
            ExecuteFinish?.Invoke(source, new ExecuteFinishEventArgs(ExecuteStatus.Abort));
        }

        /// <summary>
        /// Aborts execution
        /// </summary>
        protected void Abort() => Abort(this);
    }
}
