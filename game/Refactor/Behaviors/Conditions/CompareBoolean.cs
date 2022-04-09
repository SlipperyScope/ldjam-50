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
    /// Checks a boolean
    /// </summary>
    public class CompareBoolean : ICondition
    {
        [Export]
        private Boolean Match = true;

        [Export]
        private String Value;

        public Boolean CanRun(IRobot robot) => Value is not null && robot.Vars.ReadOrDefault<object>(Value) is Boolean val && val == Match;
    }
}
