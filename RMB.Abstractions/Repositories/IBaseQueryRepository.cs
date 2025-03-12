using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Repositories
{
    public interface IBaseQueryRepository<TEntity> : IBaseQuery<TEntity> 
        where TEntity : BaseModel
    {
    }
}
