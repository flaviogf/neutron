using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace Neutron.Core
{
    public sealed class CreateEvent : IRequest<Result>
    {
        public CreateEvent(Guid id, string name, DateTime? target)
        {
            Id = id;
            Name = name;
            Target = target;
        }

        public Guid Id { get; }

        public string Name { get; }

        public DateTime? Target { get; }
    }
}
