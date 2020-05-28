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
        protected readonly IRepository<T> ContactRepository;

        protected BaseService(
            IRepository<T> contactRepository)
        {
            ContactRepository = contactRepository;
        }

        public virtual Task<Maybe<T>> CreateAsync(T entity)
        {
            return ContactRepository.CreateAsync(entity);
        }

        public virtual Task<Maybe<T>> GetByIdAsync(string id)
        {
            return ContactRepository.GetAsync(id);
        }

        public virtual Task<Maybe<IEnumerable<T>>> GetAllAsync()
        {
            return ContactRepository.GetAllAsync();
        }

        public virtual Task<Maybe<PaginationResult<T>>> GetPaginationResultAsync(ISpecification<T> specification)
        {
            return ContactRepository.GetPaginationResultAsync(specification);
        }

        public virtual async Task<Maybe<T>> UpdateAsync(T entity)
        {
            var existingEntity = await ContactRepository.GetAsync(entity.Id);
            if (!existingEntity.HasValue)
                return Maybe<T>.Nothing;

            return await ContactRepository.UpdateAsync(entity);
        }

        public virtual Task<Maybe<T>> DeleteByIdAsync(string id)
        {
            return ContactRepository.DeleteByIdAsync(id);
        }

        public Task ClearAsync()
        {
            return ContactRepository.ClearAsync();
        }
    }
}