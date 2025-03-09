using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    public interface IBaseAddRepository<TEntity> : IBaseAdd<TEntity> where TEntity : BaseModel
    {
    }
}
