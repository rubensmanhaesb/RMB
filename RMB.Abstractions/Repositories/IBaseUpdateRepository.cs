using RMB.Abstractions.Shared.Contracts;
using RMB.Abstractions.Shared.Models;

namespace RMB.Abstractions.Repositories
{
    /// <summary>
    /// Defines the contract for updating entities in a repository.
    /// This interface extends IBaseUpdate to enforce the update operation for entities 
    /// that inherit from BaseModel.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that inherits from BaseModel.</typeparam>

    public interface IBaseUpdateRepository<TEntity> : IBaseUpdate<TEntity> where TEntity : BaseModel
    {
    }
}
