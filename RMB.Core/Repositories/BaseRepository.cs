using Microsoft.EntityFrameworkCore;
using RMB.Abstractions.Models;
using RMB.Abstractions.Repositories;

namespace RMB.Core.Repositories
{
    public abstract class BaseRepository<TEntity> 
        : IBaseAddRepository<TEntity>, 
        IBaseUpdateRepository<TEntity>, 
        IBaseDeleteRepository<TEntity>, 
        IBaseQueryRepository<TEntity>
        where TEntity : BaseModel
    {

        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;

        }

        public async virtual Task<List<TEntity>> GetAll()
            => await _dbContext.Set<TEntity>().ToListAsync();

        public void Dispose()=> _dbContext.Dispose();
    }
}
