using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Neutron.Application;
using Neutron.Web.Database;
using Neutron.Web.Models;

namespace Neutron.Web.Infrastructure
{
    public class EFEventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EFEventRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Add(CreateEventOutput output)
        {
            Event @event = _mapper.Map<Event>(output);

            await _context.Events.AddAsync(@event);

            return Result.Success();
        }
    }
}
