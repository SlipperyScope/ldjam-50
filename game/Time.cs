using Godot;
using ldjam50.Refactor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Documentation
// TODO: Time on the physics thread
namespace ldjam50
{

    public class NotifyExpiredEventArgs : EventArgs
    {
        public UInt64 Ticket { get; private set; }

        public NotifyExpiredEventArgs(UInt64 ticket)
        {
            Ticket = ticket;
        }
    }

    public class Time : Node
    {
        public delegate void NotifyCallback();
        public delegate void NotifyExpiredEventHandler(object sender, NotifyExpiredEventArgs e);

        public event NotifyExpiredEventHandler NotifyExpired;

        private class TicketManager
        {
            private UInt64 Ticket;
            public UInt64 Next => Ticket++;
        }

        private TicketManager Ticket = new();


        private record Notify
        {
            public Notify(UInt64 ticket)
            {
                Ticket = ticket;
            }

            public UInt64 Ticket { get; private set; }
            public Single Created { get; set; }
            public Single Time { get; set; }
            public Single Interval { get; set; }
            public Int32 RemainingCalls { get; set; }
            public NotifyCallback Callback { get; set; }
        }

        /// <summary>
        /// Number of seconds since game start
        /// </summary>
        public Single Seconds { get; private set; }

        private List<Notify> Notifies => _Notifies ??= new List<Notify>();
        private List<Notify> _Notifies;

        /// <summary>
        /// Adds a one-shot notifier
        /// </summary>
        /// <param name="notifyTime">Time until notify</param>
        /// <param name="callback">Notification callback</param>
        /// <returns>Ticket</returns>
        public UInt64 AddOneshot(Single notifyTime, NotifyCallback callback) => AddRecurring(notifyTime, 1, callback);
        
        [Obsolete]
        public UInt64 AddNotify(Single notifyTime, NotifyCallback callback) => AddRecurring(notifyTime, 1, callback);

        /// <summary>
        /// Adds a recurring notifier
        /// </summary>
        /// <param name="interval">Time between notifications</param>
        /// <param name="count">Number of notifications</param>
        /// <param name="callback">Notification callback</param>
        /// <returns>Ticket</returns>
        public UInt64 AddRecurring(Single interval, Int32 count, NotifyCallback callback) => AddRecurring(interval, interval, count, callback);

        /// <summary>
        /// Adds a recurring notifier
        /// </summary>
        /// <param name="delay">Time until first notification</param>
        /// <param name="interval">Time between notifications</param>
        /// <param name="count">Number of notifications</param>
        /// <param name="callback">Notification callback</param>
        /// <returns>Ticket</returns>
        public UInt64 AddRecurring(Single delay, Single interval, Int32 count, NotifyCallback callback)
        {
            if (count < 1) throw new GDErrException("Count must be greater than 0");
            if (interval < 0) throw new GDErrException("Interval must be 0 or greater");

            return AddNotify(delay, interval, count, callback);
        }

        /// <summary>
        /// Adds a looping notifyer. Note: notifier must be manually removed
        /// </summary>
        /// <param name="interval">Time between notifications</param>
        /// <param name="callback">Notification callback</param>
        /// <returns>Ticket</returns>
        public UInt64 AddLooping(Single interval, NotifyCallback callback) => AddNotify(interval, interval, Int32.MaxValue, callback);

        /// <summary>
        /// Adds a looping notifier. Note: notifier must be manually removed
        /// </summary>
        /// <param name="delay">Delay before first notification</param>
        /// <param name="interval">Time between notifications</param>
        /// <param name="callback">Notification callback</param>
        /// <returns>Ticket</returns>
        public UInt64 AddLooping(Single delay, Single interval, NotifyCallback callback) => AddNotify(delay, interval, Int32.MaxValue, callback);

        /// <summary>
        /// Adds a notify
        /// </summary>
        /// <returns>Ticket</returns>
        private UInt64 AddNotify(Single delay, Single interval, Int32 count, NotifyCallback callback)
        {
            var ticket = Ticket.Next;
            var notify = new Notify(ticket)
            {
                Time = Seconds + delay,
                Interval = interval,
                RemainingCalls = count,
                Callback = callback
            };
            Notifies.Add(notify);
            return ticket;
        }

        /// <summary>
        /// Checks if a notify exits
        /// </summary>
        /// <param name="ticket">Notify ticket</param>
        /// <returns>True if it exists</returns>
        public Boolean QueryNotify(UInt64 ticket) => Notifies.Any(n => n.Ticket == ticket);

        /// <summary>
        /// Removes a notify
        /// </summary>
        /// <param name="ticket">notify ticket</param>
        /// <returns>True if it was removed</returns>
        public Boolean RemoveNotify(UInt64 ticket) => RemoveNotify(Notifies.FirstOrDefault(n => n.Ticket == ticket));

        /// <summary>
        /// Schedules a notify to run on the next update instead of its normal time
        /// </summary>
        /// <param name="ticket">Notify ticket</param>
        public void ForceNotify(UInt64 ticket)
        {
            var notify = Notifies.First(n => n.Ticket == ticket);
            Notifies.Remove(notify);
            Notifies.Add(notify with { Time = Seconds });
        }

        /// <summary>
        /// Removes a notify
        /// </summary>
        /// <param name="notify">Notify to remove</param>
        /// <returns>True if it was removed</returns>
        private Boolean RemoveNotify(Notify notify)
        {
            if (Notifies.Remove(notify) is true)
            {
                NotifyExpired?.Invoke(this, new NotifyExpiredEventArgs(notify.Ticket));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Queues multiple, successive notifies
        /// </summary>
        /// <param name="time"></param>
        /// <param name="callbacks"></param>
        public void QueueNotify(Single time, IEnumerable<NotifyCallback> callbacks) {
            var wait = time;
            foreach(var cb in callbacks) {
                AddOneshot(wait, cb);
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

                if (notify.RemainingCalls <= 1)
                {
                    RemoveNotify(notify);
                }
                else
                {
                    Notifies.Remove(notify);
                    Notifies.Add(notify with { RemainingCalls = notify.RemainingCalls - 1, Time = notify.Time + notify.Interval });
                }
            }
        }
    }
}
