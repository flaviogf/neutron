using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neutron.Application;
using Neutron.Core;

namespace Neutron.Infrastructure
{
    public class EFEventRepository : IEventRepository
    {
        private readonly NeutronDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public EFEventRepository(NeutronDbContext context, IHttpContextAccessor accessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<Result> Add(Event @event)
        {
            string userId = _userManager.GetUserId(_accessor.HttpContext.User);

            _context.Entry(@event).Property("UserId").CurrentValue = userId;

            await _context.Events.AddAsync(@event);

            return Result.Success();
        }

        public IEnumerable<Event> FindAll()
        {
            string userId = _userManager.GetUserId(_accessor.HttpContext.User);

            return _context.Events.Where(it => EF.Property<string>(it, "UserId") == userId);
        }

        public async Task<Maybe<Event>> FindById(Guid id)
        {
            string userId = _userManager.GetUserId(_accessor.HttpContext.User);

            Maybe<Event> maybeEvent = await _context.Events.FirstOrDefaultAsync(it => it.Id == id && EF.Property<string>(it, "UserId") == userId);

            return maybeEvent;
        }

        public Task<Result> Remove(Event @event)
        {
            _context.Events.Remove(@event);

            var result = Result.Success();

            return Task.FromResult(result);
        }

        public Task<int> Count()
        {
            string userId = _userManager.GetUserId(_accessor.HttpContext.User);

            return _context.Events.CountAsync(it => EF.Property<string>(it, "UserId") == userId);
        }
    }
}
