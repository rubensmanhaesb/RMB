using RMB.Abstractions.Shared.Models;
using System.Linq.Expressions;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines the contract for querying entities in the system.
    /// This interface ensures that implementing classes provide asynchronous methods 
    /// for retrieving all entities or a specific entity by its identifier.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseQuery<TEntity> : IDisposable
        where TEntity : BaseModel
    {
        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity with the specified identifier, or null if not found.</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetOneByAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
