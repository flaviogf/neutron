using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace Neutron.Application
{
    public sealed class CreateEventInput : IRequest<Result<CreateEventOutput>>
    {
        public CreateEventInput(Guid id, string name, DateTime? target)
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
