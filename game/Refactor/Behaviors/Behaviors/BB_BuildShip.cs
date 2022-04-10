using Godot;
using ldjam50.Refactor.Entities.BigBad;
using ldjam50.Refactor.Entities.BigBad.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors.Behaviors
{
    public class BB_BuildShip : Behavior
    {
        [Export]
        private PackedScene Template;

        public override void Execute(IRobot robot)
        {
            if (Template is null)
            {
                Abort();
            }
            else if (robot is BigBad bigBad && Template is not null && Template.Instance() is BigBadTemplate template)
            {
                bigBad.BuildShip(template);
                Finish(true); 
            }
            else
            {
                Finish(false);
            }
        }
    }
}
