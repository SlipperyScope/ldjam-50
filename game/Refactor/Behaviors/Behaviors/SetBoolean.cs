using Godot;
using ldjam50.Refactor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Behaviors
{
    public class SetBoolean : Behavior
    {
        [Export]
        private String VarName;

        [Export]
        private Boolean Value = true;

        [Export]
        private Boolean Force = false;

        public override void Execute(IRobot robot)
        {
            if (VarName is null)
            {
                "Var name is null".Warn();
                Abort();
            }
            else if (Force is false)
            {
                Finish(robot.Vars.TryWrite(VarName, Value));
            }
            else if (robot.Vars.Exists(VarName) is false)
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
