using System.Linq.Expressions;

namespace SMS.Infrastructure.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);

        Task<ICollection<TEntity>> GetAllAsync(bool asNoTracking = true);

        Task<ICollection<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

        Task<TEntity> AddAsync(TEntity entity);

        Task<List<TEntity>> AddAllAsync(List<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<(ICollection<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);

        Task SaveAsync();
    }
}
