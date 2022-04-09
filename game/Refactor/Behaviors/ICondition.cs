using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    /// <summary>
    /// Defines a behavior condition
    /// </summary>
    public interface ICondition
    {
        public Boolean CanRun(IRobot robot);
    }
}
