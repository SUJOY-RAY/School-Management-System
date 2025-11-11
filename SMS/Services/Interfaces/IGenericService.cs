using System.Linq.Expressions;

namespace SMS.Services.Interfaces
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<ICollection<TEntity>> GetAllAsync();
        Task<ICollection<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}
