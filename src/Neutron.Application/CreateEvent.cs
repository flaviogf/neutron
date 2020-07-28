using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Neutron.Domain;

namespace Neutron.Application
{
    public class CreateEvent : IRequestHandler<CreateEventInput, Result<CreateEventOutput>>
    {
        private readonly IEventRepository _eventRepository;

        public CreateEvent(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Result<CreateEventOutput>> Handle(CreateEventInput input, CancellationToken cancellationToken)
        {
            Result<Guid> idOrError = IdOrError(input.Id);

            Result<string> nameOrError = NameOrError(input.Name);

            var validationResult = Result.Combine(idOrError, nameOrError);

            if (validationResult.IsFailure)
            {
                return Result.Failure<CreateEventOutput>(validationResult.Error);
            }

            DateTime target = TargetOrDefault(input.Target);

            var @event = new Event(idOrError.Value, nameOrError.Value, target);

            var output = new CreateEventOutput(@event);

            Result addResult = await _eventRepository.Add(output);

            if (addResult.IsFailure)
            {
                return Result.Failure<CreateEventOutput>(addResult.Error);
            }

            return Result.Success(output);
        }

        private Result<Guid> IdOrError(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Guid>("Id cannot be empty");
            }

            return Result.Success(id);
        }

        private Result<string> NameOrError(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<string>("Name cannot be empty");
            }

            return Result.Success(name);
        }

        private DateTime TargetOrDefault(DateTime? target)
        {
            if (target.HasValue)
            {
                return target.Value;
            }

            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        }
    }
}
