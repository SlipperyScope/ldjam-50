using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50.Refactor.Behaviors
{
    public class RobotVars
    {
        private Dictionary<String, object> vars = new();

        public T ReadOrDefault<T>(String name) => vars.ContainsKey(name) ? vars[name] is T t ? t : default(T) : throw new RobotValueNotFound($"{name} not found");
        public System.Object Read(String name) => vars.ContainsKey(name) ? vars[name] : throw new RobotValueNotFound($"{name} not found");
        public void Write(String name, System.Object value) => vars[name] = value;
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
        public Boolean Exists(String name) => vars.ContainsKey(name);
        public Boolean Exists<T>(String name) => vars.ContainsKey(name) && vars[name] is T;
    }
}
