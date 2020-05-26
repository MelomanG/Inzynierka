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

        public virtual async Task<Maybe<T>> GetAsync(string id)
        {
            var entity = (await HexadoDbContext.Set<T>()
                    .FindAsync(id))
                .ToMaybe();

            if (!entity.HasValue)
                return entity;

            HexadoDbContext.Entry(entity.Value).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<Maybe<T>> GetSingleOrMaybeAsync(Expression<Func<T, bool>> predicate)
        {
            return (await HexadoDbContext.Set<T>()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(predicate))
                .ToMaybe();
        }

        public virtual async Task<Maybe<T>> GetSingleOrMaybeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = HexadoDbContext.Set<T>();
            queryable = includes
                .Aggregate(queryable,
                    (current, include) =>
                        current.Include(include));

            return (await queryable
                    .AsNoTracking()
                    .SingleOrDefaultAsync(predicate))
                .ToMaybe();
        }

        public virtual async Task<Maybe<T>> GetSingleOrMaybeAsync(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            IQueryable<T> queryable = HexadoDbContext.Set<T>();
            queryable = includes
                .Aggregate(queryable,
                    (current, include) =>
                        current.Include(include));

            return (await queryable
                    .AsNoTracking()
                    .SingleOrDefaultAsync(predicate))
                .ToMaybe();
        }

        public virtual async Task<Maybe<IEnumerable<T>>> GetAllAsync()
        {
            return (await HexadoDbContext.Set<T>()
                    .AsNoTracking()
                    .ToListAsync())
                .AsEnumerable()
                .ToMaybe();
        }

        public virtual async Task<Maybe<IEnumerable<T>>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = HexadoDbContext.Set<T>();
            queryable = includes
                .Aggregate(queryable,
                    (current, include) =>
                        current.Include(include));

            return (await queryable
                    .AsNoTracking()
                    .ToListAsync())
                .AsEnumerable()
                .ToMaybe();
        }

        public virtual async Task<Maybe<IEnumerable<T>>> GetAllAsync(ISpecification<T> specification)
        {
            return (await HexadoDbContext.Set<T>()
                    .AsNoTracking()
                    .ApplySpecification(specification)
                    .ToListAsync())
                .AsEnumerable()
                .ToMaybe();
        }

        public virtual async Task<Maybe<PaginationResult<T>>> GetPaginationResultAsync(ISpecification<T> specification)
        {
            return (await HexadoDbContext.Set<T>()
                    .AsNoTracking()
                    .AsPaginationResultAsync(specification))
                .ToMaybe();
        }
    }
}