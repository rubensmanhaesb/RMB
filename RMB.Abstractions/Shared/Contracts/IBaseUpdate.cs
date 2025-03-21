using RMB.Abstractions.Entities;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines the contract for updating entities in the system.
    /// This interface ensures that implementing classes provide an asynchronous method 
    /// to update existing entities in the data store.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseUpdate<TEntity> : IDisposable
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>The updated entity.</returns>
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
