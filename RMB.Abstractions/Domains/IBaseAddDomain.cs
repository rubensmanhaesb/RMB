using RMB.Abstractions.Core;
using RMB.Abstractions.Entities;
using RMB.Abstractions.Models;

namespace RMB.Abstractions.Domains
{

    public interface IBaseAddDomain<TEntity> : IBaseAdd<TEntity> where TEntity : BaseModel
    {

    }
}
