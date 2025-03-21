using RMB.Abstractions.Entities;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines the contract for adding entities in the system.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to add new entities to the data store.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseAdd<TEntity> : IDisposable
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);
    }
}
