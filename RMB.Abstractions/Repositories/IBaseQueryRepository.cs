using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for querying entities in a repository.
    /// This interface extends <see cref="IBaseQuery{TEntity}"/> to enforce query operations 
    /// for entities that inherit from <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseQueryRepository<TEntity> : IBaseQuery<TEntity> 
        where TEntity : BaseEntity
    {
    }
}
