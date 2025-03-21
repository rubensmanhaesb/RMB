using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for querying entities within the domain layer.
    /// This interface extends <see cref="IBaseQuery{TEntity}"/> to enforce query operations 
    /// for entities that inherit from <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseEntity"/>.</typeparam>
    public interface IBaseQueryDomain<TEntity> : IBaseQuery<TEntity>
        where TEntity : BaseEntity
    {
    }
}
