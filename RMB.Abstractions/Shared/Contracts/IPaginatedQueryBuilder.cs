using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace RMB.Abstractions.Shared.Contracts
{
    public interface IPaginatedQueryBuilder<TEntity, TProjection>
        where TEntity : class
    {
        IPaginatedQueryBuilder<TEntity, TProjection> WithPredicate(Expression<Func<TEntity, bool>> predicate);
        IPaginatedQueryBuilder<TEntity, TProjection> WithInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        IPaginatedQueryBuilder<TEntity, TProjection> WithOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IPaginatedQueryBuilder<TEntity, TProjection> WithGroupBy(Expression<Func<TEntity, object>> groupBy);
        IPaginatedQueryBuilder<TEntity, TProjection> WithSelectGroupBy(Expression<Func<IGrouping<object, TEntity>, TProjection>> selectGroupBy);
        IPaginatedQueryBuilder<TEntity, TProjection> WithSelect(Expression<Func<TEntity, TProjection>> select);
    }


}
