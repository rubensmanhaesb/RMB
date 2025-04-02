using RMB.Abstractions.Entities;
using System.Linq.Expressions;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines a base contract for querying entities from a data source.
    /// Provides methods for retrieving all entities, filtering by predicate, and fetching by ID.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that extends <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseQuery<TEntity> : IDisposable
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Asynchronously retrieves all entities from the data source.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of all entities.</returns>
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves all entities that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The filter expression to apply.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of entities that match the predicate.</returns>
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves the first entity that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The filter expression to apply.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The first matching entity, or null if no match is found.</returns>
        Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    }
}
