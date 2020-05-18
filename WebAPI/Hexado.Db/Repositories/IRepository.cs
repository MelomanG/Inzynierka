using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Db.Repositories
{
    public interface IRepository<T>: IReadOnlyRepository<T> where T : class, IBaseEntity
    {
        Task<Maybe<T>> CreateAsync(T entity);
        Task<Maybe<T>> UpdateAsync(T entity);
        Task<Maybe<T>> DeleteAsync(T entity);
        Task<Maybe<T>> DeleteByIdAsync(string id);
        Task ClearAsync();
    }
}