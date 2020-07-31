using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Neutron.Core;

namespace Neutron.Application
{
    public class CreateEventHandler : IRequestHandler<CreateEvent, Result>
    {
        private readonly IEventRepository _eventRepository;

        public CreateEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Result> Handle(CreateEvent request, CancellationToken cancellationToken)
        {
            Result result = await CheckIfMoreEventsCanBeAdded();

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Result<Event> eventResult = await CreateEvent(request);

            if (eventResult.IsFailure)
            {
                return Result.Failure(eventResult.Error);
            }

            eventResult = await AddEvent(eventResult.Value);

            if (eventResult.IsFailure)
            {
                return Result.Failure(eventResult.Error);
            }

            return Result.Success();
        }

        private async Task<Result> CheckIfMoreEventsCanBeAdded()
        {
            int numberOfEvents = await _eventRepository.Count();

            if (numberOfEvents >= 4)
            {
                return Result.Failure("Each user can just have four events for waiting");
            }

            return Result.Success();
        }

        private Task<Result<Event>> CreateEvent(CreateEvent request)
        {
            DateTime target = TargetOrDefault(request);

            var @event = new Event(request.Id, request.Name, target);

            var result = Result.Success(@event);

            return Task.FromResult(result);
        }

        private async Task<Result<Event>> AddEvent(Event @event)
        {
            Result result = await _eventRepository.Add(@event);

            if (result.IsFailure)
            {
                return Result.Failure<Event>(result.Error);
            }

            return Result.Success(@event);
        }

        private DateTime TargetOrDefault(CreateEvent request)
        {
            if (!request.Target.HasValue)
            {
                return DateTime.Today.AddDays(1);
            }

            return request.Target.Value;
        }
    }
}
