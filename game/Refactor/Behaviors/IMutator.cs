using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    /// <summary>
    /// Defines a mutator.
    /// <para>Mutators run after a behavior to respond to their status</para>
    /// </summary>
    public interface IMutator
    {
        /// <summary>
        /// Mutates the a behavior status
        /// </summary>
        /// <param name="robot">Robot</param>
        /// <param name="status">Previous status</param>
        /// <returns>New status</returns>
        public BehaviorStatus Mutate(IRobot robot, in BehaviorStatus status);
    }
}
