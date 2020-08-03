using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neutron.Core
{
    public class Countdown
    {
        public Countdown(Event @event)
        {
            Event = @event;
        }

        public Event Event { get; }

        public event EventHandler Changed;

        public async Task Start(CancellationToken token)
        {
            while (Event.HasNotArrived && !token.IsCancellationRequested)
            {
                OnChange();

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private void OnChange()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
