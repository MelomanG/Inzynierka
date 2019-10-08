using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T: class, IBaseEntity
    {
        protected readonly HexadoDbContext HexadoDbContext;

        protected Repository(HexadoDbContext hexadoDbContext)
        {
            HexadoDbContext = hexadoDbContext;
        }

        public async Task<Maybe<T>> GetAsync(string id)
        {
            var entity = (await HexadoDbContext.Set<T>().FindAsync(id)).ToMaybe();
            if (!entity.HasValue)
                return entity;

            HexadoDbContext.Entry(entity.Value).State = EntityState.Detached;
            return entity;
        }

        public async Task<Maybe<IEnumerable<T>>> GetAllAsync()
        {
            return (await HexadoDbContext.Set<T>().AsNoTracking().ToListAsync()).AsEnumerable().ToMaybe();
        }

        public async Task<Maybe<T>> FirstOrMaybe(Expression<Func<T, bool>> predicate)
        {
            return (await HexadoDbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate)).ToMaybe();
        }

        public async Task<Maybe<T>> CreateAsync(T entity)
        {
            await HexadoDbContext.Set<T>().AddAsync(entity);
            await HexadoDbContext.SaveChangesAsync();
            return entity.ToMaybe();
        }

        public async Task<Maybe<T>> UpdateAsync(T entity)
        {
            HexadoDbContext.Entry(entity).State = EntityState.Modified;
            await HexadoDbContext.SaveChangesAsync();

            await HexadoDbContext.Entry(entity).ReloadAsync();
            HexadoDbContext.Entry(entity).State = EntityState.Detached;
            return entity.ToMaybe();
        }

        public async Task<Maybe<T>> DeleteAsync(string id)
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