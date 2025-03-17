using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for updating entities within the domain layer.
    /// This interface extends <see cref="IBaseUpdate{TEntity}"/> to enforce update operations 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseUpdateDomain<TEntity> : IBaseUpdate<TEntity>
        where TEntity : BaseModel
    {
    }
}
