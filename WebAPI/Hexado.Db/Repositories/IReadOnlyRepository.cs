using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Db.Repositories
{
    public interface IReadOnlyRepository<T> where T : class, IBaseEntity
    {
        Task<Maybe<T>> GetAsync(string id);
        Task<Maybe<T>> GetSingleOrMaybeAsync(Expression<Func<T, bool>> predicate);
        Task<Maybe<T>> GetSingleOrMaybeAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> include);
        Task<Maybe<IEnumerable<T>>> GetAllAsync();
        Task<Maybe<IEnumerable<T>>> GetAllAsync(Expression<Func<T, object>> include);
        Task<Maybe<PaginationResult<T>>> GetPaginationResultAsync(ISpecification<T> specification);
    }
}