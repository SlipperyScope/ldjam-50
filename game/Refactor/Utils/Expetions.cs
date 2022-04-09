using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Utils
{
    /// <summary>
    /// Exception that prints a message to the godot console
    /// </summary>
    public class GDErrException : Exception
    {
        public GDErrException(String message)
        {
            GD.PrintErr($"{nameof(GDErrException)}: {message}");
        }

        protected GDErrException() { }
    }

    /// <summary>
    /// Indicates a child node was not found
    /// </summary>
    public class ChildNotFoundException : GDErrException
    {
        public ChildNotFoundException(String message)
        {
            GD.PrintErr($"{nameof(ChildNotFoundException)}: {message}");
        }
    }

    /// <summary>
    /// Indicates a node has the incorrect parent
    /// </summary>
    public class MisparentedNodeException : GDErrException
    {
        public MisparentedNodeException(String message)
        {
            GD.PrintErr($"{nameof(MisparentedNodeException)}: {message}");
        }
    }

    /// <summary>
    /// Indicates a node cast is invalid
    /// </summary>
    public class InvalidNodeCastException : GDErrException
    {
        public InvalidNodeCastException(String message)
        {
            GD.PrintErr($"{nameof(InvalidNodeCastException)}: {message}");
        }
    }

    /// <summary>
    /// Indicates a node has an invalid child
    /// </summary>
    public class InvalidChildException : GDErrException
    {
        public InvalidChildException(String message)
        {
            GD.PrintErr($"{nameof(InvalidChildException)}: {message}");
        }
    }

    public class RobotValueNotFound : GDErrException
    {
        public RobotValueNotFound(String message)
        {
            GD.PrintErr($"{nameof(RobotValueNotFound)}: {message}");
        }
    }
}
