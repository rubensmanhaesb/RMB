using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Shared.Contracts.Paginations
{
    public interface IBasePaginatedQueryRepository<TEntity>: IBasePaginatedQuery<TEntity> where TEntity : BaseEntity
    {

    }



}
