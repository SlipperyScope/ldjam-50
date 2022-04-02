using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ldjam50
{
    public class Time : Node
    {
        public delegate void TimeNotifyCallback();
        private record TimeNotify(Single Created, Single Time, TimeNotifyCallback callback);
        
        /// <summary>
        /// Number of seconds since game start
        /// </summary>
        public Single Seconds { get; private set; }

        private List<TimeNotify> Notifies => _Notifies ??= new List<TimeNotify>();
        private List<TimeNotify> _Notifies;

        /// <summary>
        /// Adds a notify
        /// </summary>
        /// <param name="time">Time to notify</param>
        /// <param name="callback">Callback to invoke</param>
        /// <param name="relative">Time is relative to current time</param>
        public void AddNotify(Single time, TimeNotifyCallback callback, Boolean relative = true) => Notifies.Add(new TimeNotify(Seconds, relative ? Seconds + time : time, callback));

        /// <summary>
        /// Process
        /// </summary>
        public override void _Process(Single delta)
        {
            Seconds += delta;

            foreach( var notify in Notifies.Where(n => Seconds >= n.Time))
            {
                notify.callback();
                Notifies.Remove(notify);
            }
        }
    }
}
