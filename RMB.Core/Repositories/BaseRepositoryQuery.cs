using Microsoft.EntityFrameworkCore;
using RMB.Abstractions.Models;
using RMB.Abstractions.Repositories;

namespace RMB.Core.Repositories
{
    public class BaseRepositoryQuery<TEntity> : IBaseQueryRepository<TEntity> where TEntity : BaseModel
    {
        private readonly DbContext _dbContext;

        public BaseRepositoryQuery(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose() => _dbContext.Dispose();

        public async Task<List<TEntity>>? GetAll() => await _dbContext.Set<TEntity>().ToListAsync();
    }
}
