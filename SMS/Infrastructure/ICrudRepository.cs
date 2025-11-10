using SMS.Models;
using System.Linq.Expressions;

namespace SMS.Infrastructure
{
    public interface ICrudRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<ICollection<TEntity>> GetAllAsync();
        Task<(ICollection<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddAllAsync(List<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
