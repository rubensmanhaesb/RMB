using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for updating entities within the domain layer.
    /// This interface extends <see cref="IBaseUpdate{TEntity}"/> to enforce update operations 
    /// for entities that inherit from <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseUpdateDomain<TEntity> : IBaseUpdate<TEntity>
        where TEntity : BaseEntity
    {
    }
}
