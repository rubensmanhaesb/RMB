using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for deleting entities in a repository.
    /// This interface extends <see cref="IBaseDelete{TEntity}"/> to enforce delete operations 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseDeleteRepository<TEntity> : IBaseDelete<TEntity> where TEntity : BaseModel
    {
    }
}
