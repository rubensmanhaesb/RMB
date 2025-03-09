using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Core
{
    public interface IBaseUpdate<TEntity> : IDisposable where TEntity : BaseModel
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
