using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for querying entities in a repository.
    /// This interface extends <see cref="IBaseQuery{TEntity}"/> to enforce query operations 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseQueryRepository<TEntity> : IBaseQuery<TEntity> 
        where TEntity : BaseModel
    {
    }
}
