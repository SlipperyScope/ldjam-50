using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    /// <summary>
    /// Variables for a robot behavior tree
    /// </summary>
    public class RobotVars
    {
        private Dictionary<String, object> vars = new();

        /// <summary>
        /// Returns <paramref name="name"/> or the default value of <typeparamref name="T"/> if name doesn't exist
        /// </summary>
        /// <typeparam name="T">Type of var</typeparam>
        /// <param name="name">Name of var</param>
        /// <returns>Var or default</returns>
        public T ReadOrDefault<T>(String name) => vars.ContainsKey(name) && vars[name] is T t ? t : default;

        /// <summary>
        /// Returns <paramref name="name"/>
        /// </summary>
        /// <param name="name">Name of var</param>
        /// <returns>Var</returns>
        /// <exception cref="RobotVarNotFound">Var does not exist</exception>
        public System.Object Read(String name) => vars.ContainsKey(name) ? vars[name] : throw new RobotVarNotFound($"{name} not found");

        /// <summary>
        /// Returns <paramref name="name"/> as <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type of var</typeparam>
        /// <param name="name">Name of var</param>
        /// <returns>Var as <typeparamref name="T"/></returns>
        /// <exception cref="RobotValueInvalidCast">Var exists but is not <typeparamref name="T"/></exception>
        public T Read<T>(String name) => vars.ContainsKey(name) && vars[name] is T t ? t : throw new RobotValueInvalidCast($"Cannot cast {vars[name].GetType()} {name} to {typeof(T)}");

        /// <summary>
        /// Sets <paramref name="value"/> to <paramref name="value"/> as <typeparamref name="T"/> or default, returns if read was successful
        /// </summary>
        /// <typeparam name="T">Type of var</typeparam>
        /// <param name="name">Name of var</param>
        /// <param name="value">Value to write var</param>
        /// <returns>True if value exists and was <typeparamref name="T"/></returns>
        public Boolean TryRead<T>(String name, out T value)
        {
            var success = vars.TryGetValue(name, out var obj);
            if (success && obj is T t)
            {
                value = t;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Sets <paramref name="value"/> to <paramref name="value"/>
        /// </summary>
        /// <param name="name">Name of var</param>
        /// <param name="value">New value</param>
        /// <exception cref="RobotVarNotFound">No var named <paramref name="value"/></exception>
        public void Write(String name, System.Object value)
        {
            if (vars.ContainsKey(name))
            {
                vars[name] = value;
            }
            else
            {
                throw new RobotVarNotFound($"{name} not found");
            }
        }

        /// <summary>
        /// Attempts to set <paramref name="value"/> to <paramref name="value"/>
        /// </summary>
        /// <param name="name">Name of var</param>
        /// <param name="value">New value</param>
        /// <returns>True if write was successful</returns>
        public Boolean TryWrite(String name, System.Object value)
        {
            if (vars.ContainsKey(name) && value.GetType() == vars[name].GetType())
            {
                vars[name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Checks if <paramref name="name"/> exists
        /// </summary>
        /// <param name="name">Name of var</param>
        /// <returns>True if <paramref name="name"/> exists</returns>
        public Boolean Exists(String name) => vars.ContainsKey(name);

        /// <summary>
        /// Checks if <paramref name="name"/> exists and is <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Var type</typeparam>
        /// <param name="name">Name of var</param>
        /// <returns>True if <typeparamref name="name"/> exists and is <typeparamref name="T"/></returns>
        public Boolean Exists<T>(String name) => vars.ContainsKey(name) && vars[name] is T;

        /// <summary>
        /// Writes a new var
        /// </summary>
        /// <param name="name">Name of var</param>
        /// <param name="value">Var value</param>
        /// <returns>True if it was written</returns>
        public Boolean WriteNew(String name, Object value)
        {
            if (vars.ContainsKey(name)) return false;
            vars.Add(name, value);
            return true;
        }
    }
}
