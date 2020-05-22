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
        Task<Maybe<T>> GetSingleOrMaybeAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<Maybe<IEnumerable<T>>> GetAllAsync();
        Task<Maybe<IEnumerable<T>>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<Maybe<IEnumerable<T>>> GetAllAsync(ISpecification<T> specification);
        Task<Maybe<PaginationResult<T>>> GetPaginationResultAsync(ISpecification<T> specification);
    }
}