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
        private record TimeNotify
        {
            public TimeNotify(Single created, Single time, TimeNotifyCallback callback)
            {
                Created = created;
                Time = time;
                Callback = callback;
            }

            public Single Created { get; set; }
            public Single Time { get; set; }
            public TimeNotifyCallback Callback { get; set; }
        }



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
        public void AddNotify(Single time, TimeNotifyCallback callback, Boolean relative = true)
        {
            Notifies.Add(new TimeNotify(Seconds, relative ? Seconds + time : time, callback));
        }

        public void QueueNotify(Single time, IEnumerable<TimeNotifyCallback> callbacks) {
            var wait = time;
            foreach(var cb in callbacks) {
                AddNotify(wait, cb);
                wait += time;
            }
        }

        /// <summary>
        /// Process
        /// </summary>
        public override void _Process(Single delta)
        {
            Seconds += delta;

            foreach( var notify in Notifies.Where(n => Seconds >= n.Time).ToList())
            {
                notify.Callback();
                Notifies.Remove(notify);
            }
        }
    }
}
