using RMB.Abstractions.Core;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for deleting entities within the domain layer.
    /// This interface extends <see cref="IBaseDelete{TEntity}"/> to enforce the delete operation 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseDeleteDomain<TEntity> : IBaseDelete<TEntity>
        where TEntity : BaseModel
    {
    }
}
