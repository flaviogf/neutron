using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Event> FindAll()
        {
            return _context.Events;
        }

        public async Task<Maybe<Event>> FindById(Guid id)
        {
            Maybe<Event> maybeEvent = await _context.Events.FirstOrDefaultAsync(it => it.Id == id);

            return maybeEvent;
        }
    }
}
