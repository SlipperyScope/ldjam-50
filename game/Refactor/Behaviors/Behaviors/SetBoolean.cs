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
    /// Sets a boolean
    /// </summary>
    public class SetBoolean : Behavior
    {
        [Export]
        private String VarName;

        [Export]
        private Boolean Value = true;

        /// <summary>
        /// Write the value if it doesn't exist, but not if it exists as something other than a boolean
        /// </summary>
        [Export]
        private Boolean Force = false;

        public override void Execute(IRobot robot)
        {
            GetPath().Print();
            if (VarName is null)
            {
                "Var name is null".Warn();
                Abort();
            }
            else if (robot.Vars.TryWrite(VarName, Value))
            {
                Finish(true);
            }
            else if (Force is true && robot.Vars.Exists(VarName) is false)
            {
                robot.Vars.Write(VarName, Value);
                Finish(true);
            }
            else
            {
                Finish(false);
            }
        }
    }
}
