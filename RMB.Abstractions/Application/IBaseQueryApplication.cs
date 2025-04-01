using System.Linq.Expressions;

namespace RMB.Abstractions.Application
{
    /// <summary>
    /// Represents the application layer contract for querying data using a request model and returning a result model.
    /// Supports fetching all records, filtering by predicate, and retrieving by ID.
    /// </summary>
    /// <typeparam name="TRequest">The type used to filter or query the data (typically the entity or a query model).</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the query (typically a DTO).</typeparam>
    public interface IBaseQueryApplication<TRequest, TResult> : IDisposable
    {
        /// <summary>
        /// Asynchronously retrieves all results.
        /// </summary>
        /// <returns>A list of all results.</returns>
        Task<List<TResult>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a result by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to retrieve.</param>
        /// <returns>The result if found; otherwise, null.</returns>
        Task<TResult?> GetByIdAsync(Guid id);

        /// <summary>
        /// Asynchronously retrieves all results that match the specified filter.
        /// </summary>
        /// <param name="predicate">The filter expression to apply.</param>
        /// <returns>A list of results that match the filter.</returns>
        Task<List<TResult>> GetAllAsync(Expression<Func<TRequest, bool>> predicate);

        /// <summary>
        /// Asynchronously retrieves the first result that matches the specified filter.
        /// </summary>
        /// <param name="predicate">The filter expression to apply.</param>
        /// <returns>The first matching result, or null if no match is found.</returns>
        Task<TResult?> GetOneByAsync(Expression<Func<TRequest, bool>> predicate);
    }
}
