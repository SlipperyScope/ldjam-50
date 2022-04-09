using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Conditions
{
    /// <summary>
    /// Tests conditions by always passing or failing
    /// </summary>
    public class Test : Node, ICondition
    {
        [Export]
        private Boolean Pass = true;

        public Boolean CanRun(IRobot robot) => Pass;
    }
}
