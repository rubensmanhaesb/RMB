using Microsoft.EntityFrameworkCore;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Repositories;
using RMB.Abstractions.Shared.Contracts.Paginations;
using RMB.Abstractions.Shared.Contracts.Paginations.Requests;
using RMB.Abstractions.Shared.Contracts.Paginations.Responses;
using RMB.Core.Paginations.Extensions;
using RMB.Core.Paginations.Services;
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


        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;

        }

        public async Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);

        public virtual async Task<TEntity>? GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);


        public async virtual Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await _dbContext.Set<TEntity>()
                .AsNoTracking() 
                .Where(predicate)
                .ToListAsync(cancellationToken);            


        public async virtual Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            =>await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate,cancellationToken);

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
