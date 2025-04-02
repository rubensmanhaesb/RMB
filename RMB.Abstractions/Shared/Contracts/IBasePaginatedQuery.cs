using RMB.Abstractions.Shared.Contracts.Paginations.Requests;
using RMB.Abstractions.Shared.Contracts.Paginations.Responses;
using System.Linq.Expressions;


namespace RMB.Abstractions.Shared.Contracts
{

    /// <summary>
    /// Defines a contract for executing paginated queries over a given entity type,
    /// with support for filtering, projection, grouping, and ordering.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being queried.</typeparam>
    public interface IBasePaginatedQuery<TEntity>
    {
        /// <summary>
        /// Executes a paginated query with optional filtering, eager loading, ordering, grouping, and projection.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projected result returned by the query.</typeparam>
        /// <param name="predicate">Optional filter to apply to the query.</param>
        /// <param name="paginationRequest">Pagination parameters such as page number and page size.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <param name="include">Optional function to include related entities (eager loading).</param>
        /// <param name="orderBy">Optional ordering function for the query results.</param>
        /// <param name="groupBy">Optional expression to group the results by a specific field.</param>
        /// <param name="selectGroupBy">Optional projection to apply on grouped data.</param>
        /// <param name="select">Optional projection to apply on non-grouped data.</param>
        /// <returns>A task representing the asynchronous operation, containing a paginated result with the projected items.</returns>
        Task<PaginatedResult<TProjection>> GetPaginatedAsync<TProjection>(
            Expression<Func<TEntity, bool>>? predicate,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Expression<Func<TEntity, object>>? groupBy = null,
            Expression<Func<IGrouping<object, TEntity>, TProjection>>? selectGroupBy = null,
            Expression<Func<TEntity, TProjection>>? select = null);
    }

}
