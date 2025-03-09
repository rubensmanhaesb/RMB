using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    public interface IBaseDeleteRepository<TEntity> : IBaseDelete<TEntity> where TEntity : BaseModel
    {
    }
}
