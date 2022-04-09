using Godot;
using ldjam50.Refactor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Behaviors
{
    /// <summary>
    /// Waits a certain amount of time before finishing
    /// <para>Success: After delay
    /// <br />Failure: Not possible
    /// <br />Abort:   Not possible
    /// </para>
    /// </summary>
    public class Delay : Behavior
    {
        [Export]
        private Single Time = 1f;

        public override void Execute(IRobot robot)
        {
            if (Debug)
            {
                Global.Time.Seconds.Printf("Started waiting at ", "0.00");
                Global.Time.AddOneshot(Time, () =>
                {
                    Global.Time.Seconds.Printf("Finished waiting at", "0.00");
                    Finish(true);
                });
            }
            else
            {
                Global.Time.AddOneshot(Time, () => Finish(true));
            }
        }
    }
}
