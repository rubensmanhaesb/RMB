using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    /// <summary>
    /// Defines the contract for adding entities within the domain layer.
    /// This interface extends <see cref="IBaseAdd{TEntity}"/> to enforce the add operation 
    /// for entities that inherit from <see cref="BaseModel"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from <see cref="BaseModel"/>.</typeparam>
    public interface IBaseAddDomain<TEntity> : IBaseAdd<TEntity>
        where TEntity : BaseModel
    {
    }
}
