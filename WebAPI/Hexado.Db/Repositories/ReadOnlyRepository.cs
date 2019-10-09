using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Speczilla.Extensions;
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

        public async Task<Maybe<PaginationResult<T>>> GetPaginationResultAsync(ISpecification<T> specification)
        {
            return (await HexadoDbContext.Set<T>()
                .AsNoTracking()
                .AsPaginationResultAsync(specification))
                .ToMaybe();
        }
    }
}