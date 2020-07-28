using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Neutron.Core;

namespace Neutron.Application
{
    public interface IEventRepository
    {
        Task<Result> Add(Event @event);
    }
}
