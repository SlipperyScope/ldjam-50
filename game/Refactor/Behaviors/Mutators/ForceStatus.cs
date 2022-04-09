using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Mutators
{
    /// <summary>
    /// Tests mutators by forcing a status
    /// </summary>
    public class ForceStatus : Node, IMutator
    {
        [Export]
        private BehaviorStatus Status;

        public BehaviorStatus Mutate(IRobot robot, in BehaviorStatus status) => Status;
    }
}
