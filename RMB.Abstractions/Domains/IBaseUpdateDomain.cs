using RMB.Abstractions.Core;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{
    public interface IBaseUpdateDomain<TEntity> : IBaseUpdate<TEntity> where TEntity : BaseModel
    {
    }
}
