using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Core
{
    public interface IBaseAdd<TEntity> : IDisposable where TEntity : BaseModel
    {
        Task<TEntity> AddAsync(TEntity entity);

    }
}
