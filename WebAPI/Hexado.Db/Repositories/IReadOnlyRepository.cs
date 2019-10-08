using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Db.Repositories
{
    public interface IReadOnlyRepository<T> where T : class, IBaseEntity
    {
        Task<Maybe<T>> GetAsync(string id);
        Task<Maybe<IEnumerable<T>>> GetAllAsync();
        Task<Maybe<IEnumerable<T>>> GetManyAsync(int skip, int take);
        Task<int> CountAsync();
    }
}