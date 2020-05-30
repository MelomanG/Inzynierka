using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Db.Repositories.Specific
{
    public interface IEventRepository : IRepository<Event>
    {
        Maybe<IEnumerable<string>> GetCities();
    }

    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }

        public Maybe<IEnumerable<string>> GetCities()
        {
            var result = HexadoDbContext.Set<Event>()
                .GroupBy(p => new
                {
                    p.Address.City
                })
                .Select(x => x.Key.City)
                .ToList()
                .AsEnumerable()
                .ToMaybe();

            return result;
        }
    }
}