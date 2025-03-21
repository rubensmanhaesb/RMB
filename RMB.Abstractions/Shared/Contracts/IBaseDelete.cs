using RMB.Abstractions.Entities;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines the contract for deleting entities in the system.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to delete entities from the data store.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseDelete<TEntity> : IDisposable
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>The deleted entity.</returns>
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}
