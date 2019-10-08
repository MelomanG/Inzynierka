using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;

namespace Hexado.Core.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T: class, IBaseEntity
    {
        private readonly IRepository<T> _repository;

        protected BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<Maybe<T>> CreateAsync(T entity)
        {
            return _repository.CreateAsync(entity);
        }

        public Task<Maybe<T>> GetByIdAsync(string id)
        {
            return _repository.GetAsync(id);
        }

        public Task<Maybe<IEnumerable<T>>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Maybe<IEnumerable<T>>> GetPagedResults()
        {
        }

        public async Task<Maybe<T>> UpdateAsync(T entity)
        {
            var existingEntity = await _repository.GetAsync(entity.Id);
            if (!existingEntity.HasValue)
                return Maybe<T>.Nothing;

            return await _repository.UpdateAsync(entity);
        }

        public Task<Maybe<T>> DeleteByIdAsync(string id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}