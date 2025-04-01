using RMB.Abstractions.Entities;
using RMB.Abstractions.Shared.Contracts;


namespace RMB.Abstractions.Domains
{

    public interface IBasePaginatedQueryDomain<TEntity> : IBasePaginatedQuery<TEntity> where TEntity : BaseEntity
    {
    }

}
