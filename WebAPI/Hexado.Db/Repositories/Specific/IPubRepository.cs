using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Db.Repositories.Specific
{
    public interface IPubRepository : IRepository<Pub>
    {
        Maybe<IEnumerable<string>> GetCities();
    }

    public class PubRepository : Repository<Pub>, IPubRepository
    {
        public PubRepository(
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }

        public Maybe<IEnumerable<string>> GetCities()
        {
            var result = HexadoDbContext.Set<Pub>()
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