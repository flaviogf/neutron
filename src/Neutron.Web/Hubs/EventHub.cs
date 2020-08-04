using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using Neutron.Application;
using Neutron.Core;

namespace Neutron.Web.Hubs
{
    public class EventHub : Hub<IEventClient>
    {
        private readonly IEventRepository _eventRepository;

        public EventHub(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Tick(string userId, Guid eventId)
        {
            Maybe<Event> maybeEvent = await _eventRepository.FindById(eventId);

            if (maybeEvent.HasNoValue)
            {
                return;
            }

            await Clients.User(userId).Tack(maybeEvent.Value);
        }
    }
}
