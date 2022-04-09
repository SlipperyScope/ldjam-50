using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using ldjam50.Refactor.Interfaces;

namespace ldjam50.Refactor.Behaviors.Behaviors
{
    /// <summary>
    /// Literally just aborts. For testing, mostly
    /// <para>Success: Not possible
    /// <br />Failure: Not possible
    /// <br />Abort:   Always, immediately
    /// </para></summary>
    public class Abort : Behavior
    {
        public override void Execute(IRobot robot)
        {
            Abort();
        }
    }
}
