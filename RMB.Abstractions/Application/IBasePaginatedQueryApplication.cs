using RMB.Abstractions.Shared.Contracts.Paginations.Requests;
using System.Linq.Expressions;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Represents the application layer contract for performing paginated queries using a request model.
    /// Provides support for filtering, projection, ordering, and grouping over a paginated result set.
    /// </summary>
    /// <typeparam name="TRequest">The type of the entity or queryable model used for the query.</typeparam>
    /// <typeparam name="TResult">The type of the result returned after executing the paginated query.</typeparam>
    public interface IBasePaginatedQueryApplication<TRequest, TResult>
    {
        /// <summary>
        /// Asynchronously executes a paginated query with optional filtering, eager loading, ordering, grouping, and projection.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projected result returned by the query.</typeparam>
        /// <param name="predicate">Optional filter to apply to the query.</param>
        /// <param name="paginationRequest">Pagination parameters including page number and page size.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <param name="include">Optional function to include related entities (eager loading).</param>
        /// <param name="orderBy">Optional ordering function for the result set.</param>
        /// <param name="groupBy">Optional expression to group results by a specific field.</param>
        /// <param name="selectGroupBy">Optional projection to apply on grouped data.</param>
        /// <param name="select">Optional projection to apply on non-grouped data.</param>
        /// <returns>A task representing the asynchronous operation that returns a paginated result.</returns>
        Task<TResult> GetPaginatedAsync<TProjection>(
            Expression<Func<TRequest, bool>>? predicate,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken,
            Func<IQueryable<TRequest>, IQueryable<TRequest>>? include = null,
            Func<IQueryable<TRequest>, IOrderedQueryable<TRequest>>? orderBy = null,
            Expression<Func<TRequest, object>>? groupBy = null,
            Expression<Func<IGrouping<object, TRequest>, TProjection>>? selectGroupBy = null,
            Expression<Func<TRequest, TProjection>>? select = null);
    }
}
