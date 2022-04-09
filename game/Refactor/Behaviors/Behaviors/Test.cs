using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Behaviors
{
    /// <summary>
    /// Immediately returns a status
    /// </summary>
    public class Test : Behavior
    {
        [Export]
        private BehaviorStatus Status;

        public override void Execute(IRobot robot) => OnBehaviorFinished(Status);
    }
}
