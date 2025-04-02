using System.Linq.Expressions;

namespace RMB.Abstractions.Shared.Contracts
{
    /// <summary>
    /// Defines a fluent interface for building paginated query options, including filtering,
    /// eager loading, ordering, grouping, and projection logic.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <typeparam name="TProjection">The type of the result projection.</typeparam>
    public interface IPaginatedQueryBuilder<TEntity, TProjection>
        where TEntity : class
    {
        /// <summary>
        /// Sets a filter predicate to be applied to the query.
        /// </summary>
        /// <param name="predicate">The filtering condition.</param>
        /// <returns>The builder instance.</returns>
        IPaginatedQueryBuilder<TEntity, TProjection> WithPredicate(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Specifies related entities to include for eager loading.
        /// </summary>
        /// <param name="include">The include function.</param>
        /// <returns>The builder instance.</returns>
        IPaginatedQueryBuilder<TEntity, TProjection> WithInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);

        /// <summary>
        /// Specifies the ordering to be applied to the query.
        /// </summary>
        /// <param name="orderBy">The ordering logic.</param>
        /// <returns>The builder instance.</returns>
        IPaginatedQueryBuilder<TEntity, TProjection> WithOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        /// <summary>
        /// Specifies a grouping expression to group the results.
        /// </summary>
        /// <param name="groupBy">The grouping logic.</param>
        /// <returns>The builder instance.</returns>
        IPaginatedQueryBuilder<TEntity, TProjection> WithGroupBy(Expression<Func<TEntity, object>> groupBy);

        /// <summary>
        /// Specifies a projection for grouped results.
        /// </summary>
        /// <param name="selectGroupBy">The group-by projection.</param>
        /// <returns>The builder instance.</returns>
        IPaginatedQueryBuilder<TEntity, TProjection> WithSelectGroupBy(Expression<Func<IGrouping<object, TEntity>, TProjection>> selectGroupBy);

        /// <summary>
        /// Specifies a projection for flat (non-grouped) results.
        /// </summary>
        /// <param name="select">The projection expression.</param>
        /// <returns>The builder instance.</returns>
        IPaginatedQueryBuilder<TEntity, TProjection> WithSelect(Expression<Func<TEntity, TProjection>> select);
    }
}
