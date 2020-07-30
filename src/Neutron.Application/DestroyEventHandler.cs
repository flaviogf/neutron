using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Neutron.Core;

namespace Neutron.Application
{
    public class DestroyEventHandler : IRequestHandler<DestroyEvent, Result>
    {
        private readonly IEventRepository _eventRepository;

        public DestroyEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Result> Handle(DestroyEvent request, CancellationToken cancellationToken)
        {
            Result<Event> eventResult = await GetEvent(request);

            if (eventResult.IsFailure)
            {
                return Result.Failure(eventResult.Error);
            }

            eventResult = await RemoveEvent(eventResult.Value);

            if (eventResult.IsFailure)
            {
                return Result.Failure(eventResult.Error);
            }

            return Result.Success();
        }

        public async Task<Result<Event>> GetEvent(DestroyEvent request)
        {
            Maybe<Event> maybeEvent = await _eventRepository.FindById(request.Id);

            if (maybeEvent.HasNoValue)
            {
                return Result.Failure<Event>("Event does not exist");
            }

            Event @event = maybeEvent.Value;

            return Result.Success(@event);
        }

        public async Task<Result<Event>> RemoveEvent(Event @event)
        {
            Result result = await _eventRepository.Remove(@event);

            if (result.IsFailure)
            {
                return Result.Failure<Event>(result.Error);
            }

            return Result.Success(@event);
        }
    }
}
