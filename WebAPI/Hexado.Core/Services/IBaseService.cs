using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Core.Services
{
    public interface IBaseService<T> where T : class, IBaseEntity
    {
        Task<Maybe<T>> CreateAsync(T entity);
        Task<Maybe<T>> GetByIdAsync(string id);
        Task<Maybe<IEnumerable<T>>> GetAllAsync();
        Task<Maybe<T>> UpdateAsync(T entity);
        Task<Maybe<T>> DeleteByIdAsync(string id);
    }
}