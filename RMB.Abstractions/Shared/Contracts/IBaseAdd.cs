using RMB.Abstractions.Entities;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines the contract for adding new entities to the system.
    /// Implementing classes must provide an asynchronous method for persisting new entities in the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseAdd<TEntity> : IDisposable
        where TEntity : BaseEntity
    {
        /// <summary>
        /// Asynchronously adds a new entity to the data store.
        /// </summary>
        /// <param name="entity">The entity instance to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The added entity instance after persistence.</returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
