using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Hexado.Speczilla;

namespace Hexado.Core.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T: class, IBaseEntity
    {
        protected readonly IRepository<T> Repository;

        protected BaseService(
            IRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual Task<Maybe<T>> CreateAsync(T entity)
        {
            return Repository.CreateAsync(entity);
        }

        public virtual Task<Maybe<T>> GetByIdAsync(string id)
        {
            return Repository.GetAsync(id);
        }

        public virtual Task<Maybe<IEnumerable<T>>> GetAllAsync()
        {
            return Repository.GetAllAsync();
        }

        public virtual Task<Maybe<PaginationResult<T>>> GetPaginationResultAsync(ISpecification<T> specification)
        {
            return Repository.GetPaginationResultAsync(specification);
        }

        public virtual async Task<Maybe<T>> UpdateAsync(T entity)
        {
            var existingEntity = await Repository.GetAsync(entity.Id);
            if (!existingEntity.HasValue)
                return Maybe<T>.Nothing;

            return await Repository.UpdateAsync(entity);
        }

        public virtual Task<Maybe<T>> DeleteByIdAsync(string id)
        {
            return Repository.DeleteByIdAsync(id);
        }
    }
}