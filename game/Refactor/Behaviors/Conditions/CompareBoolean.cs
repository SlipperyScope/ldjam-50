using Godot;
using ldjam50.Refactor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Conditions
{
    /// <summary>
    /// Checks a boolean against a robotvars value
    /// <para>Pass: Values are the same
    /// <br />Fail: Values are not the same</para>
    /// </summary>
    public class CompareBoolean : Node, ICondition
    {
        [Export]
        private String VarName;

        [Export]
        private Boolean Match = true;

        public Boolean CanRun(IRobot robot) => VarName is not null && robot.Vars.ReadOrDefault<object>(VarName) is Boolean val && val == Match;
    }
}
