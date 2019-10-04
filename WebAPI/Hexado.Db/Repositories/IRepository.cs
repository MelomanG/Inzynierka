using System.Threading.Tasks;

namespace Hexado.Db.Repositories
{
    public interface IRepository<T>
    {
        Task UpdateAsync(T entity);
    }
}