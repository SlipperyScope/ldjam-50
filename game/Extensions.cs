using Godot;
using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ldjam50
{
    public static class Extensions
    {
        /// <summary>
        /// Print to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Print<T>(this T t) => GD.Print(t);

        /// <summary>
        /// Print to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        public static void Print<T>(this T t, String message) => GD.Print($"{message} {t}");

        /// <summary>
        /// Print to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="format">Formatting to apply</param>
        /// <param name="provider">Format provider</param>
        public static void Printf(this IFormattable t, String format, IFormatProvider provider = null) => GD.Print(t.ToString(format, provider));

        /// <summary>
        /// Print to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        /// <param name="format">Format string</param>
        /// <param name="provider">Format provider</param>
        public static void Printf(this IFormattable t, String message, String format, IFormatProvider provider = null) => GD.Print($"{message} {t.ToString(format, provider)}");


        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Warn<T>(this T t) => GD.PushWarning($"{t}");

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        public static void Warn<T>(this T t, String message) => GD.PushWarning($"{message} {t}");

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="format">Formatting to apply</param>
        /// <param name="provider">Format provider</param>
        public static void Warnf(this IFormattable t, String format, IFormatProvider provider = null) => GD.PushWarning(t.ToString(format, provider));

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        /// <param name="format">Format string</param>
        /// <param name="provider">Format provider</param>
        public static void Warnf(this IFormattable t, String message, String format, IFormatProvider provider = null) => GD.PushWarning($"{message} {t.ToString(format, provider)}");
        #region Node

        /// <summary>
        /// Gets a list of all descendant nodes, depth first
        /// </summary>
        /// <param name="node"></param>
        /// <returns>List of nodes</returns>
        public static List<Node> GetDescendants(this Node node)
        {
            List<Node> list = new();

            node.GetChildren().ToList().ForEach(child =>
            {
                list.Add(child);
                list.AddRange(child.GetDescendants());
            });

            return list;
        }

        /// <summary>
        /// Gets a list of all descentants that are type <typeparamref name="T"/>, depth first
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="node"></param>
        /// <returns>List of of type <typeparamref name="T"/></returns>
        public static List<T> GetDescendants<T>(this Node node) where T : Node => (from child in node.GetDescendants()
                                                                                   where child is T
                                                                                   select child as T).ToList();

        /// <summary>
        /// Frees a node, removing it from scene tree if applicable
        /// </summary>
        /// <param name="node"></param>
        public static void Kill(this Node node)
        {
            node.GetParent()?.RemoveChild(node);
            node.QueueFree();
        }

        /// <summary>
        /// Gets a reference to a child node
        /// </summary>
        /// <typeparam name="T">Type of node</typeparam>
        /// <param name="nameOverride">Overrides the name of the child, default is child's type</param>
        /// <returns>Reference to the child</returns>
        /// <exception cref="ChildNotFoundException">Thrown if the node is not found</exception>
        public static T GetChild<T>(this Node from, String nameOverride = null) where T : Node
            => from.GetNode(nameOverride ?? typeof(T).Name) as T
            ?? throw new ChildNotFoundException($"{from.Name} has no child of type {typeof(T)} named {nameOverride ?? typeof(T).Name}");

        #endregion

        #region Node2D

        /// <summary>
        /// Rect2 representing the visible screen
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Rect2</returns>
        public static Rect2 ScreenRect(this Node2D node)
        {
            var transform = node.GetCanvasTransform().AffineInverse();
            var rect = node.GetViewportRect();
            return new Rect2(transform * rect.Position, rect.Size);
        }

        /// <summary>
        /// x position of screen left
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Single ScreenLeft(this Node2D node) => node.ScreenRect().Position.x;

        /// <summary>
        /// x position of screen right
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Single ScreenRight(this Node2D node) => node.ScreenRect().End.x;

        /// <summary>
        /// y position of screen top
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Single ScreenTop(this Node2D node) => node.ScreenRect().Position.y;

        /// <summary>
        /// y position of screen bottom
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Single ScreenBottom(this Node2D node) => node.ScreenRect().End.y;

        /// <summary>
        /// Width of screen
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Single ScreenWidth(this Node2D node) => node.ScreenRect().Size.x;

        /// <summary>
        /// Height of screen
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Single ScreenHeight(this Node2D node) => node.ScreenRect().Size.y;

        #endregion

        #region Array
        /// <summary>
        /// Creates a list of nodes from a godot array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ignoreMismatch">Ignore entrys that are not of type Node</param>
        /// <returns>List of nodes</returns>
        public static List<Node> ToList(this Godot.Collections.Array array, Boolean ignoreMismatch = true) => array.ToList<Node>(ignoreMismatch);

        /// <summary>
        /// Creates a list of type <typeparamref name="T"/> from a godot array
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="array"></param>
        /// <param name="ignoreMismatch">Ignore entrys that are not of type <typeparamref name="T"/></param>
        /// <returns>List of type <typeparamref name="T"/></returns>
        /// <exception cref="ArrayTypeMismatchException">Array contained an item that was not type T, and <paramref name="ignoreMismatch"/> is false</exception>
        public static List<T> ToList<T>(this Godot.Collections.Array array, Boolean ignoreMismatch = true)
        {
            List<T> list = new();

            foreach (var item in array)
            {
                if (item is T type)
                {
                    list.Add(type);
                }
                else if (ignoreMismatch is false)
                {
                    GD.PrintErr($"{item} is not type {typeof(T)}");
                    throw new ArrayTypeMismatchException($"{item} is not type {typeof(T)}");
                }
            }

            return list;
        }
        #endregion

        #region Boolean

        /// <summary>
        /// Print to console
        /// </summary>
        /// <param name="boolean"></param>
        /// <param name="ifTrue">Message if true</param>
        /// <param name="ifFalse">Message if false</param>
        public static void Print(this Boolean boolean, String ifTrue, String ifFalse) => GD.Print(boolean ? ifTrue : ifFalse);

        /// <summary>
        /// Print to console
        /// </summary>
        /// <param name="boolean"></param>
        /// <param name="message">Message to prepend</param>
        /// <param name="ifTrue">Message if true</param>
        /// <param name="ifFalse">Message if false</param>
        public static void Print(this Boolean boolean, String message, String ifTrue, String ifFalse) => GD.Print($"{message} {(boolean ? ifTrue : ifFalse)}");

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns a random element from an IEnumerable
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable"></param>
        /// <returns>Element</returns>
        public static T Random<T>(this IEnumerable<T> enumerable) => enumerable.ElementAt((Int32)(GD.Randi() % enumerable.Count()));

        /// <summary>
        /// Shuffles a list into a random order
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">List</param>
        /// <returns>New shuffled list</returns>
        public static List<T> Shuffled<T>(this List<T> list)
        {
            List<T> from = list.ToList();
            List<T> shuffled = new();
            Int32 i;
            while(from.Count > 0)
            {
                i = (Int32)(GD.Randi() % from.Count);
                shuffled.Add(from[i]);
                from.RemoveAt(i);
            }
            return shuffled;
        }

        #endregion

        #region Vector2

        /// <summary>
        /// Determines if the vector is adjecant to <paramref name="other"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="other">Other vector</param>
        /// <returns>True if they are adjacent</returns>
        public static Boolean AjacentTo(this Vector2 from, Vector2 other)
        {
            var difference = from - other;
            return difference == Vector2.Up || difference == Vector2.Down || difference == Vector2.Left || difference == Vector2.Right;
        }

        #endregion
    }
}
