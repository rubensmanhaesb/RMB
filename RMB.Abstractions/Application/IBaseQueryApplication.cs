using System.Linq.Expressions;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Defines a contract for querying data in the application layer using a request model and returning result models.
    /// Supports retrieving all records, filtering by predicates, and fetching by unique identifiers.
    /// </summary>
    /// <typeparam name="TRequest">The type used for querying (typically the entity or a query model).</typeparam>
    /// <typeparam name="TResult">The type of the result returned (typically a DTO).</typeparam>
    public interface IBaseQueryApplication<TRequest, TResult>  
    {
        /// <summary>
        /// Asynchronously retrieves all available results.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A list of all result items.</returns>
        Task<List<TResult>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a result by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>The result item if found; otherwise, null.</returns>
        Task<TResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves all results that match the specified filter expression.
        /// </summary>
        /// <param name="predicate">The filter expression to apply.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A list of results that satisfy the filter condition.</returns>
        Task<List<TResult>> GetAllAsync(Expression<Func<TRequest, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves the first result that matches the specified filter expression.
        /// </summary>
        /// <param name="predicate">The filter expression to apply.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>The first matching result, or null if no match is found.</returns>
        Task<TResult?> GetOneByAsync(Expression<Func<TRequest, bool>> predicate, CancellationToken cancellationToken);
    }
}
