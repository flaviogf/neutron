using System;

namespace Neutron.Core
{
    public class Event
    {
        public Event(Guid id, string name, DateTime target)
        {
            Id = id;
            Name = name;
            Target = target;
        }

        public Guid Id { get; }

        public string Name { get; }

        public DateTime Target { get; }

        public TimeSpan TimeLeft
        {
            get
            {
                TimeSpan timeLeft = Target - DateTime.Now;

                if (timeLeft <= TimeSpan.Zero)
                {
                    return TimeSpan.Zero;
                }

                return timeLeft;
            }
        }

        public bool HasArrived
        {
            get
            {
                return TimeLeft <= TimeSpan.Zero;
            }
        }

        public bool HasNotArrived
        {
            get
            {
                return !HasArrived;
            }
        }
    }
}
