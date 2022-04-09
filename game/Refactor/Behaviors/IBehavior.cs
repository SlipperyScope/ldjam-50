using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    /// <summary>
    /// Defines a behavior
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Handles behavior finished events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void BehaviorFinishedEventHandler(IBehavior sender, BehaviorFinishedEventArgs e);

        /// <summary>
        /// Args for a behavior finished event
        /// </summary>
        public class BehaviorFinishedEventArgs : EventArgs
        {
            /// <summary>
            /// Args from child event
            /// </summary>
            public readonly BehaviorFinishedEventArgs InnerArgs;

            /// <summary>
            /// Child sender
            /// </summary>
            public readonly IBehavior InnerSender;

            /// <summary>
            /// Finish status
            /// </summary>
            public readonly BehaviorStatus Status;

            /// <summary>
            /// Creates a new behavior finished args
            /// </summary>
            /// <param name="status">Finish status</param>
            public BehaviorFinishedEventArgs(BehaviorStatus status)
            {
                Status = status;
            }

            /// <summary>
            /// Creates a new behavior finished args
            /// </summary>
            /// <param name="status">Finsih status</param>
            /// <param name="innerSender">Preivous sender</param>
            /// <param name="innerArgs">Previous args</param>
            public BehaviorFinishedEventArgs(BehaviorStatus status, IBehavior innerSender, BehaviorFinishedEventArgs innerArgs) : this(status)
            {
                InnerSender = innerSender;
                InnerArgs = innerArgs;
            }
        }

        /// <summary>
        /// Behavior has finished
        /// </summary>
        public event BehaviorFinishedEventHandler BehaviorFinished;

        /// <summary>
        /// Conditions to run before execution
        /// </summary>
        public List<ICondition> Conditions { get; }

        /// <summary>
        /// Mutators to run after execution
        /// </summary>
        public List<IMutator> Mutators { get; }

        public Boolean CanRun(IRobot robot);
        public BehaviorStatus Mutate(IRobot robot, BehaviorStatus status);

        /// <summary>
        /// Executes the behavior
        /// </summary>
        public void Execute(IRobot robot);
    }
}
