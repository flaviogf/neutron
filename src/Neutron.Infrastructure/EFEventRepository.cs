using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Neutron.Application;
using Neutron.Core;

namespace Neutron.Infrastructure
{
    public class EFEventRepository : IEventRepository
    {
        private readonly NeutronDbContext _context;

        public EFEventRepository(NeutronDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Add(Event @event)
        {
            await _context.Events.AddAsync(@event);

            return Result.Success();
        }
    }
}
