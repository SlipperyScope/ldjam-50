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
    public class BehaviorRoot : Node
    {
        /// <summary>
        /// Whether behaviors should start automatically
        /// </summary>
        [Export]
        public Boolean AutoStart { get; set; } = true;

        /// <summary>
        /// Time until start, if using autostart
        /// </summary>
        [Export]
        public Single StartDelay { get; set; } = 0.5f;

        /// <summary>
        /// Ticks per second
        /// </summary>
        [Export]
        public Single TickRate { get; set; } = 0.5f;

        /// <summary>
        /// Whether the behaviors are running
        /// </summary>
        public Boolean Running { get; private set; } = false;

        /// <summary>
        /// Whether a behavior is being waited on
        /// </summary>
        public Boolean Waiting { get; private set; } = false;

        private IRobot Robot;
        private Behavior Behavior;
        private UInt64 TickTicket;

        /// <summary>
        /// Enter Tree
        /// </summary>
        /// <exception cref="MisparentedNodeException">Behavior is not on an IRobot</exception>
        public override void _EnterTree()
        {
            Robot = GetParent<IRobot>() ?? throw new MisparentedNodeException($"Parent is not {nameof(IRobot)}");
        }

        /// <summary>
        /// Ready
        /// </summary>
        /// <exception cref="InvalidChildException">Too many children</exception>
        /// <exception cref="InvalidNodeCastException">Child that is not behavior</exception>
        public override void _Ready()
        {
            if (GetChildCount() > 1) throw new InvalidChildException("Root behavior can only have one child");
            if (GetChild(0) is not Behavior behavior)
            {
                throw new InvalidNodeCastException("Child is not a behavior");
            }
            else
            {
                Behavior = behavior;
                Behavior.ExecuteFinish += Child_ExecuteFinish;
            }

            if (AutoStart is true)
            {
                Start();
            }
        }

        /// <summary>
        /// Handles execute finished events from the child behavior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Child_ExecuteFinish(System.Object sender, ExecuteFinishEventArgs e)
        {
            Waiting = false;
            if (e.Status == ExecuteStatus.Abort) $"{(sender as Node).GetPath()} aborted".Print();
        }

        /// <summary>
        /// Starts the behaviors
        /// </summary>
        public void Start()
        {
            if (Running is false)
            {
                Running = true;
                TickTicket = Global.Time.AddLooping(AutoStart ? StartDelay : 0f, TickRate, Tick);
            }
            else
            {
                "Behaviors already started".Warn();
            }
        }

        /// <summary>
        /// Ticks the behaviors
        /// </summary>
        private void Tick()
        {
            Global.Time.Seconds.Printf("Tick at: ", "0.00");
            if (Waiting is false)
            {
                //Global.Time.Seconds.Printf("Running behavior at ", "0.00");
                Waiting = true;
                Behavior.Execute(Robot);
            }
        }
    }
}
