using FluentValidation;
using RMB.Abstractions.Domains;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts.Paginations.Requests;
using RMB.Abstractions.Shared.Contracts.Paginations.Responses;
using RMB.Core.Paginations.Validations;
using RMB.Core.Repositories;
using System.Linq.Expressions;

namespace RMB.Core.Domains
{
    /// <summary>
    /// Base class for domain logic, providing CRUD operations using a repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from BaseModel.</typeparam>
    public abstract class BaseDomain<TEntity> :
        IBaseAddDomain<TEntity>,
        IBaseUpdateDomain<TEntity>,
        IBaseDeleteDomain<TEntity>,
        IBaseQueryDomain<TEntity>,
        IBasePaginatedQueryDomain<TEntity>
        where TEntity : BaseEntity
    {
        private readonly BaseRepository<TEntity> _baseRepository;

        protected BaseDomain(BaseRepository<TEntity> repository)
            => _baseRepository = repository;

        public async virtual Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
            => await _baseRepository.AddAsync(entity, cancellationToken);


        public async virtual Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
            => await _baseRepository.DeleteAsync(entity, cancellationToken);


        public void Dispose()
            => _baseRepository.Dispose();


        public async virtual Task<List<TEntity>>? GetAllAsync(CancellationToken cancellationToken)
            => await _baseRepository.GetAllAsync(cancellationToken);

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await _baseRepository.GetAllAsync(predicate, cancellationToken);

        public async Task<TEntity>? GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => await _baseRepository.GetByIdAsync(id,cancellationToken);

        public Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => _baseRepository.GetOneByAsync(predicate, cancellationToken);

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
            
            await (new PaginationRequestValidation()).ValidateAndThrowAsync(paginationRequest,  cancellationToken);

            var result = await _baseRepository.GetPaginatedAsync(
                predicate,
                paginationRequest,
                cancellationToken,
                include,
                orderBy,
                groupBy,
                selectGroupBy,
                select);

            await (new PaginationResultValidation<TProjection>()).ValidateAndThrowAsync(result, cancellationToken);

            return result;
        }

        public async virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
            => await _baseRepository.UpdateAsync(entity, cancellationToken);


    }
}
