using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Neutron.Core;

namespace Neutron.Application
{
    public interface IEventRepository
    {
        Task<Result> Add(Event @event);

        IEnumerable<Event> FindAll();

        Task<Maybe<Event>> FindById(Guid id);

        Task<Result> Remove(Event @event);

        Task<int> Count();
    }
}
