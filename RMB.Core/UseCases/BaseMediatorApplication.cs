using AutoMapper;
using MediatR;
using RMB.Abstractions.UseCases;
using RMB.Core.Domains;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Application;
using RMB.Abstractions.Shared.Contracts.Responses;
using System.Linq.Expressions;
using RMB.Abstractions.Shared.Contracts.Requests;

namespace RMB.Core.UseCases
{
    /// <summary>
    /// Abstract base class for application services that implement common CRUD operations
    /// using the Mediator pattern and AutoMapper for entity-DTO mapping.
    /// </summary>
    /// <typeparam name="TDtoCreate">The DTO used to create a new entity.</typeparam>
    /// <typeparam name="TDtoUpdate">The DTO used to update an existing entity.</typeparam>
    /// <typeparam name="TDtoDelete">The DTO used to delete an entity.</typeparam>
    /// <typeparam name="TDtoResult">The DTO returned as a result of operations.</typeparam>
    /// <typeparam name="TEntity">The domain entity type associated with the service.</typeparam>
    public abstract class BaseMediatorApplication<TDtoCreate, TDtoUpdate, TDtoDelete, TDtoResult, TEntity> :
        IBaseAddUseCase<TDtoCreate, TDtoResult>,
        IBaseUpdateUseCase<TDtoUpdate, TDtoResult>,
        IBaseDeleteUseCase<TDtoDelete, TDtoResult>,
        IBaseQueryApplication<TEntity, TDtoResult>,
        IBasePaginatedQueryApplication<TEntity, PaginatedResult<TDtoResult>>,
        IDisposable
        where TEntity : BaseEntity
    {
        private readonly BaseDomain<TEntity> _baseDomain;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes the application service with its domain logic, mediator, and mapper.
        /// </summary>
        protected BaseMediatorApplication(BaseDomain<TEntity> baseDomain, IMediator mediator, IMapper mapper)
        {
            _baseDomain = baseDomain;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Sends the create command through the mediator and returns the result DTO.
        /// </summary>
        public async Task<TDtoResult> AddAsync(TDtoCreate dto, CancellationToken cancellationToken)
            => (TDtoResult)await _mediator.Send(dto, cancellationToken);

        /// <summary>
        /// Sends the delete command through the mediator and returns the result DTO.
        /// </summary>
        public async Task<TDtoResult> DeleteAsync(TDtoDelete dto, CancellationToken cancellationToken)
            => (TDtoResult)await _mediator.Send(dto, cancellationToken);

        /// <summary>
        /// Sends the update command through the mediator and returns the result DTO.
        /// </summary>
        public async Task<TDtoResult> UpdateAsync(TDtoUpdate dto, CancellationToken cancellationToken)
            => (TDtoResult)await _mediator.Send(dto, cancellationToken);

        /// <summary>
        /// Retrieves and maps all entities from the domain to result DTOs.
        /// </summary>
        public virtual async Task<List<TDtoResult>?> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _baseDomain.GetAllAsync(cancellationToken);
            return _mapper.Map<List<TDtoResult>>(entities);
        }

        /// <summary>
        /// Retrieves a single entity by ID and maps it to a result DTO.
        /// </summary>
        public virtual async Task<TDtoResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _baseDomain.GetByIdAsync(id,cancellationToken);
            return entity != null ? _mapper.Map<TDtoResult>(entity) : default;
        }

        /// <summary>
        /// Disposes the domain layer resources, such as database connections.
        /// </summary>
        public void Dispose()
        {
            _baseDomain.Dispose();
        }

        /// <summary>
        /// Retrieves all entities matching a predicate and maps them to result DTOs.
        /// </summary>
        public virtual async Task<List<TDtoResult>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            var entities = await _baseDomain.GetAllAsync(predicate, cancellationToken);
            var result = _mapper.Map<List<TDtoResult>>(entities);
            return result;
        }

        /// <summary>
        /// Retrieves a single entity matching a predicate and maps it to a result DTO.
        /// </summary>
        public virtual async Task<TDtoResult?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            var entity = await _baseDomain.GetOneByAsync(predicate, cancellationToken);
            var result = _mapper.Map<TDtoResult>(entity);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of entities matching the provided options, 
        /// then maps the items to result DTOs.
        /// </summary>
        public virtual async Task<PaginatedResult<TDtoResult>> GetPaginatedAsync<TProjection>(
            Expression<Func<TEntity, bool>>? predicate,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Expression<Func<TEntity, object>>? groupBy = null,
            Expression<Func<IGrouping<object, TEntity>, TProjection>>? selectGroupBy = null,
            Expression<Func<TEntity, TProjection>>? select = null)
        {
            var paginatedEntities = await _baseDomain.GetPaginatedAsync<TEntity>(
                predicate,
                paginationRequest,
                cancellationToken,
                include,
                orderBy,
                groupBy);

            var mappedItems = _mapper.Map<List<TDtoResult>>(paginatedEntities.Items);

            return new PaginatedResult<TDtoResult>
            {
                Items = mappedItems,
                Pagination = paginatedEntities.Pagination
            };
        }
    }
}
