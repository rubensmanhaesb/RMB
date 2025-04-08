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
        private readonly IDbContextFactory<DbContext> _factory;
        protected DbContext? ExternalDbContext { get; set; }

        public void UseDbContext(DbContext context)
        {
            ExternalDbContext = context;
        }

        public BaseRepository(IDbContextFactory<DbContext> factory)
            => _factory = factory;

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await using var context = ExternalDbContext ?? await _factory.CreateDbContextAsync(cancellationToken);
            await context.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await using var context = ExternalDbContext ?? await _factory.CreateDbContextAsync(cancellationToken);
            context.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await using var context = ExternalDbContext ?? await _factory.CreateDbContextAsync(cancellationToken);
            context.Update(entity);
            await context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            var result= await context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
            return result;
        }

        public async virtual Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async virtual Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public void Dispose()
        {
            // DbContextFactory does not require disposal.
            if (ExternalDbContext != null)
                ExternalDbContext.Dispose();
        }

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
            var options = (PaginatedQueryBuilderService<TEntity, TProjection>)
                PaginatedQueryBuilderService<TEntity, TProjection>
                    .Create()
                    .WithPredicate(predicate!)
                    .WithInclude(include!)
                    .WithOrderBy(orderBy!)
                    .WithGroupBy(groupBy!)
                    .WithSelectGroupBy(selectGroupBy!)
                    .WithSelect(select!);

            await using var context = await _factory.CreateDbContextAsync(cancellationToken);
            return await context.Set<TEntity>()
                .AsNoTracking()
                .ToPaginatedResultAsync(
                    paginationRequest,
                    options,
                    cancellationToken);
        }
    }
}
