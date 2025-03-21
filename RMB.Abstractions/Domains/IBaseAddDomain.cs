using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for adding entities within the domain layer.
    /// This interface extends <see cref="IBaseAdd{TEntity}"/> to enforce the add operation 
    /// for entities that inherit from <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseAddDomain<TEntity> : IBaseAdd<TEntity>
        where TEntity : BaseEntity
    {
    }
}
