using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for adding entities in a repository.
    /// This interface extends <see cref="IBaseAdd{TEntity}"/> to enforce add operations 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseAddRepository<TEntity> : IBaseAdd<TEntity> where TEntity : BaseModel
    {
    }
}
