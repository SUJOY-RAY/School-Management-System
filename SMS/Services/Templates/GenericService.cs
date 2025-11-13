using SMS.Infrastructure.Interfaces;
using SMS.Services.Interfaces;
using System.Linq.Expressions;

namespace SMS.Services.Templates
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<TEntity>?> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.GetFilteredAsync(predicate);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
            return entity;
        }

        public async Task CreateRange(List<TEntity> entities)
        {
            if (entities==null)
            {
                throw new ArgumentNullException(nameof(entities));                
            }
            await _repository.AddAllAsync(entities);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;

            await _repository.DeleteAsync(id);
        }
    }
}