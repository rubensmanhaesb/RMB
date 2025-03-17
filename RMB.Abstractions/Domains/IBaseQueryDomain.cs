using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for querying entities within the domain layer.
    /// This interface extends <see cref="IBaseQuery{TEntity}"/> to enforce query operations 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseQueryDomain<TEntity> : IBaseQuery<TEntity>
        where TEntity : BaseModel
    {
    }
}
