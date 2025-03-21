using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for deleting entities in a repository.
    /// This interface extends <see cref="IBaseDelete{TEntity}"/> to enforce delete operations 
    /// for entities that inherit from <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseDeleteRepository<TEntity> : IBaseDelete<TEntity> where TEntity : BaseEntity
    {
    }
}
