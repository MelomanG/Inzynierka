using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        protected readonly HexadoDbContext HexadoDbContext;

        public Repository(HexadoDbContext hexadoDbContext)
        {
            HexadoDbContext = hexadoDbContext;
        }

        public Task UpdateAsync(T entity)
        {
            HexadoDbContext.Entry(entity).State = EntityState.Modified;
            return HexadoDbContext.SaveChangesAsync();
        }
    }
}