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
    }
}
