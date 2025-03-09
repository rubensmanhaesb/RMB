using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Core
{
    public interface IBaseDelete<TEntity> : IDisposable where TEntity : BaseModel
    {
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}
