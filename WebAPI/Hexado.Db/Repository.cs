using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db
{
    public class Repository<T> : IRepository<T>
    {
        private readonly HexadoDbContext _hexadoDbContext;

        public Repository(HexadoDbContext hexadoDbContext)
        {
            _hexadoDbContext = hexadoDbContext;
        }

        public Task UpdateAsync(T entity)
        {
            _hexadoDbContext.Entry(entity).State = EntityState.Modified;
            return _hexadoDbContext.SaveChangesAsync();
        }
    }
}