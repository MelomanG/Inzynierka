using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db.Repositories
{
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T: class, IBaseEntity
    {
        public Repository(HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }

        public virtual async Task<Maybe<T>> CreateAsync(T entity)
        {
            await HexadoDbContext.Set<T>().AddAsync(entity);
            await HexadoDbContext.SaveChangesAsync();
            return entity.ToMaybe();
        }

        public virtual async Task<Maybe<T>> UpdateAsync(T entity)
        {
            HexadoDbContext.Entry(entity).State = EntityState.Modified;
            await HexadoDbContext.SaveChangesAsync();

            await HexadoDbContext.Entry(entity).ReloadAsync();
            HexadoDbContext.Entry(entity).State = EntityState.Detached;
            return entity.ToMaybe();
        }

        public virtual async Task<Maybe<T>> DeleteAsync(string id)
        {
            var entityToDelete = await GetAsync(id);
            if (!entityToDelete.HasValue)
                return Maybe<T>.Nothing;

            HexadoDbContext.Set<T>().Remove(entityToDelete.Value);
            await HexadoDbContext.SaveChangesAsync();
            return entityToDelete;
        }
    }
}