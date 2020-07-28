using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Neutron.Application
{
    public interface IEventRepository
    {
        Task<Result> Add(CreateEventOutput output);
    }
}
