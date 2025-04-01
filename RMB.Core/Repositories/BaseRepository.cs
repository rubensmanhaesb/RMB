using Microsoft.EntityFrameworkCore;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Repositories;
using RMB.Abstractions.Repositories.Paginations;
using RMB.Abstractions.Shared.Contracts.Requests;
using RMB.Abstractions.Shared.Contracts.Responses;
using RMB.Core.Extensions;
using System.Linq.Expressions;

namespace RMB.Core.Repositories
{
    /// <summary>
    /// Provides a generic repository implementation for CRUD operations using Entity Framework Core.
    /// This repository abstracts common database operations such as adding, updating, deleting, 
    /// and retrieving entities, ensuring consistency and reusability across different domains.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from BaseModel.</typeparam>
    public abstract class BaseRepository<TEntity> 
        : IBaseAddRepository<TEntity>, 
        IBaseUpdateRepository<TEntity>, 
        IBaseDeleteRepository<TEntity>, 
        IBaseQueryRepository<TEntity>,
        IBasePaginatedQueryRepository<TEntity>
        where TEntity : BaseEntity
    {

        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
             => _dbContext = dbContext;


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

        public async Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();

        public virtual async Task<TEntity>? GetByIdAsync(Guid id)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);


        public async virtual Task<List<TEntity>> GetAllAsync()
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

        public async virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking() 
                .Where(predicate)
                .ToListAsync();            


        public async virtual Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate)
            =>await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);

        public void Dispose()
            => _dbContext.Dispose();


        public async Task<PaginatedResult<TProjection>> GetPaginatedAsync<TProjection>(
            Expression<Func<TEntity, bool>>? predicate,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Expression<Func<TEntity, object>>? groupBy = null,
            Expression<Func<IGrouping<object, TEntity>, TProjection>>? selectGroupBy = null,
            Expression<Func<TEntity, TProjection>>? select = null)
        {
            var options = (PaginatedQueryBuilder<TEntity, TProjection>)
                PaginatedQueryBuilder<TEntity, TProjection>
                    .Create()
                    .WithPredicate(predicate!)
                    .WithInclude(include!)
                    .WithOrderBy(orderBy!)
                    .WithGroupBy(groupBy!)
                    .WithSelectGroupBy(selectGroupBy!)
                    .WithSelect(select!);

            return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .ToPaginatedResultAsync(
                    paginationRequest,
                    options,              
                    cancellationToken);


        }

    }
}
