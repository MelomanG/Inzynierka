using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Db.Repositories
{
    public interface IRepository<T> where T : class, IBaseEntity
    {
        Task<Maybe<T>> GetAsync(string id);
        Task<Maybe<IEnumerable<T>>> GetAllAsync();
        Task<Maybe<T>> FirstOrMaybe(Expression<Func<T, bool>> predicate);
        Task<Maybe<T>> CreateAsync(T entity);
        Task<Maybe<T>> UpdateAsync(T entity);
        Task<Maybe<T>> DeleteAsync(string id);
    }
}