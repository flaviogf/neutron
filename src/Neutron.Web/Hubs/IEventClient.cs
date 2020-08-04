using System.Threading.Tasks;
using Neutron.Core;

namespace Neutron.Web.Hubs
{
    public interface IEventClient
    {
        Task Tack(Event @event);

        Task Start(Event @event);

        Task Stop(Event @event);
    }
}
