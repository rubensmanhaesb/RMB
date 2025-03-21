using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for adding entities in a repository.
    /// This interface extends <see cref="IBaseAdd{TEntity}"/> to enforce add operations 
    /// for entities that inherit from <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseAddRepository<TEntity> : IBaseAdd<TEntity> where TEntity : BaseEntity
    {
    }
}
