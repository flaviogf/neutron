using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Neutron.Application;
using Neutron.Core;

namespace Neutron.Web.Hubs
{
    public class EventHub : Hub<IEventClient>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventHub> _logger;
        private readonly CancellationTokenSource _tokenSource;

        public EventHub(IEventRepository eventRepository, ILogger<EventHub> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _tokenSource = new CancellationTokenSource();
        }

        public async Task Tick(Guid eventId)
        {
            Maybe<Event> maybeEvent = await _eventRepository.FindById(eventId);

            if (maybeEvent.HasNoValue)
            {
                return;
            }

            Countdown countdown = new Countdown(maybeEvent.Value);

            countdown.Changed += Notify;

            countdown.Changed += Log;

            await countdown.Start(_tokenSource.Token);
        }

        private async void Notify(object sender, EventArgs args)
        {
            Maybe<Countdown> maybeCountdown = sender as Countdown;

            if (maybeCountdown.HasNoValue)
            {
                return;
            }

            Countdown countdown = maybeCountdown.Value;

            await Clients.Caller.Tack(countdown.Event);
        }

        private void Log(object sender, EventArgs args)
        {
            Maybe<Countdown> maybeCountdown = sender as Countdown;

            if (maybeCountdown.HasNoValue)
            {
                return;
            }

            Countdown countdown = maybeCountdown.Value;

            _logger.LogInformation(countdown.Event.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            _tokenSource.Cancel();

            _tokenSource.Dispose();

            base.Dispose(disposing);
        }
    }
}
