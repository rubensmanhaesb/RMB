using RMB.Abstractions.Core;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    public interface IBaseQueryDomain<TEntity> : IBaseQuery<TEntity>  
        where TEntity : BaseModel
    {

    }
}
