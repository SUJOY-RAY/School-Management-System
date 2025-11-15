using Microsoft.EntityFrameworkCore;
using SMS.Infrastructure.Interfaces;
using SMS.Models.school_related;
using System.Linq.Expressions;

namespace SMS.Infrastructure
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

            if (entity == null)
                throw new KeyNotFoundException($"No entity found with id: {id}");

            return entity;
        }


        public async Task<ICollection<TEntity>> GetAllAsync(bool asNoTracking = true)
        {
            return asNoTracking
                ? await _dbSet.AsNoTracking().ToListAsync()
                : await _dbSet.ToListAsync();
        }

        public async Task<ICollection<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
        {
            return asNoTracking
                ? await _dbSet.AsNoTracking().Where(predicate).ToListAsync()
                : await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch(Exception e)
            {
                throw new Exception("Exception while adding", e);
            }
        }

        public async Task<List<TEntity>> AddAllAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(ICollection<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _dbSet.AsNoTracking();
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}