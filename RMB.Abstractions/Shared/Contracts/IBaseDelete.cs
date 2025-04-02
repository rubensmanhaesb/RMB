using RMB.Abstractions.Entities;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines the contract for deleting entities in the system.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to remove entities from the data store.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseDelete<TEntity> : IDisposable
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Asynchronously deletes the specified entity from the data store.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The deleted entity.</returns>
        Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
