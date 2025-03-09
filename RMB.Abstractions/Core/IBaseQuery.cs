using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Core
{
    public interface IBaseQuery<TEntity> : IDisposable where TEntity : BaseModel
    {
        Task<List<TEntity>>? GetAll();
    }
}
