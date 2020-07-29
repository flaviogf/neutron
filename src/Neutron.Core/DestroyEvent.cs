using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace Neutron.Core
{
    public sealed class DestroyEvent : IRequest<Result>
    {
        public DestroyEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
