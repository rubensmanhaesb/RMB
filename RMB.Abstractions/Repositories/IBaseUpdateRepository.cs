using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    public interface IBaseUpdateRepository<TEntity> : IBaseUpdate<TEntity> where TEntity : BaseModel
    {
    }
}
