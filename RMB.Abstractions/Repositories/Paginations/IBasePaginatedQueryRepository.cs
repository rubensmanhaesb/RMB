using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;

namespace RMB.Abstractions.Repositories.Paginations
{
    public interface IBasePaginatedQueryRepository<TEntity>: IBasePaginatedQuery<TEntity> where TEntity : BaseEntity
    {

    }



}
