using RMB.Abstractions.Core;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    public interface IBaseDeleteDomain<TEntity> : IBaseDelete<TEntity>  where TEntity : BaseModel
    {

    }
}
