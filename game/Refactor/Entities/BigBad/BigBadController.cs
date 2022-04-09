using Godot;
using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Entities.BigBad
{
    /// <summary>
    /// Controlls big bad behaviors
    /// </summary>
    public class BigBadController : Node
    {
        ///// <summary>
        ///// Gets a reference to a big bad controller child on <paramref name="parent"/>
        ///// </summary>
        ///// <param name="parent">Parent of desired controller</param>
        ///// <returns>Ref</returns>
        ///// <exception cref="ChildNotFoundException">Parent has no child named and of type "BigBadController"</exception>
        //public static BigBadController GetFrom(Node parent) => parent.GetNode<BigBadController>(nameof(BigBadController)) ?? throw new ChildNotFoundException() { GDMessage = nameof(BigBadController) };

        /// <summary>
        /// Ready
        /// </summary>
        public override void _Ready()
        {
            base._Ready();
        }
    }
}