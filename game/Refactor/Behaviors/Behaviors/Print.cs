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
    /// Prints a message to the console
    /// <para>Success: After print
    /// <br />Failure: Not possible
    /// <br />Abort:   Not possible
    /// </para>
    /// </summary>
    public class Print : Behavior
    {
        [Export]
        private String Message;

        public override void Execute(IRobot robot)
        {
            Message.Print();
            Finish(true);
        }
    }
}
