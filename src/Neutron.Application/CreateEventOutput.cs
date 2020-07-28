using System;
using Neutron.Domain;

namespace Neutron.Application
{
    public sealed class CreateEventOutput
    {
        internal CreateEventOutput(Event @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Target = @event.Target;
        }

        public Guid Id { get; }

        public string Name { get; }

        public DateTime Target { get; }
    }
}
