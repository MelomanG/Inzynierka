using System.Threading.Tasks;

namespace Hexado.Db
{
    public interface IRepository<T>
    {
        Task UpdateAsync(T entity);
    }
}