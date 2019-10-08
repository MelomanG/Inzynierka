using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db.Repositories
{
    public abstract class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class, IBaseEntity
    {
        protected readonly HexadoDbContext HexadoDbContext;

        protected ReadOnlyRepository(HexadoDbContext hexadoDbContext)
        {
            HexadoDbContext = hexadoDbContext;
        }

        public async Task<Maybe<T>> GetAsync(string id)
        {
            var entity = (await HexadoDbContext.Set<T>().FindAsync(id)).ToMaybe();
            if (!entity.HasValue)
                return entity;

            HexadoDbContext.Entry(entity.Value).State = EntityState.Detached;
            return entity;
        }

        public async Task<Maybe<IEnumerable<T>>> GetAllAsync()
        {
            return (await HexadoDbContext.Set<T>().AsNoTracking().ToListAsync()).AsEnumerable().ToMaybe();
        }

        public async Task<Maybe<IEnumerable<T>>> GetManyAsync(int skip, int take)
        {
            return (await HexadoDbContext.Set<T>().Skip(skip).Take(take).AsNoTracking().ToListAsync()).AsEnumerable().ToMaybe();
        }

        public Task<int> CountAsync()
        {
            return HexadoDbContext.Set<T>().CountAsync();
        }
    }
}