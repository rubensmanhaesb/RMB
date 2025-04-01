using RMB.Abstractions.Shared.Contracts;
using System.Linq.Expressions;

namespace RMB.Core.Repositories
{
    /// <summary>
    /// A fluent builder that configures options for executing paginated queries.
    /// It provides methods to apply filtering, including, ordering, grouping, and projection logic.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <typeparam name="TProjection">The type of the final projection returned in the result.</typeparam>
    public class PaginatedQueryBuilder<TEntity, TProjection>
        : PaginatedQueryOptions<TEntity, TProjection>, // Inherits the option storage
          IPaginatedQueryBuilder<TEntity, TProjection> // Implements the fluent interface
        where TEntity : class
    {
        /// <summary>
        /// Static factory method to create a new instance of the builder.
        /// </summary>
        public static IPaginatedQueryBuilder<TEntity, TProjection> Create()
            => new PaginatedQueryBuilder<TEntity, TProjection>();

        /// <summary>
        /// Sets the filtering expression for the query.
        /// </summary>
        public IPaginatedQueryBuilder<TEntity, TProjection> WithPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
            return this;
        }

        /// <summary>
        /// Sets the include expression to load related data eagerly.
        /// </summary>
        public IPaginatedQueryBuilder<TEntity, TProjection> WithInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            Include = include;
            return this;
        }

        /// <summary>
        /// Sets the ordering logic for the query.
        /// </summary>
        public IPaginatedQueryBuilder<TEntity, TProjection> WithOrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            OrderBy = orderBy;
            return this;
        }

        /// <summary>
        /// Sets the grouping expression to group query results.
        /// </summary>
        public IPaginatedQueryBuilder<TEntity, TProjection> WithGroupBy(Expression<Func<TEntity, object>> groupBy)
        {
            GroupBy = groupBy;
            return this;
        }

        /// <summary>
        /// Sets the projection logic to apply to grouped results.
        /// </summary>
        public IPaginatedQueryBuilder<TEntity, TProjection> WithSelectGroupBy(Expression<Func<IGrouping<object, TEntity>, TProjection>> selectGroupBy)
        {
            SelectGroupBy = selectGroupBy;
            return this;
        }

        /// <summary>
        /// Sets the projection logic to apply to flat (non-grouped) results.
        /// </summary>
        public IPaginatedQueryBuilder<TEntity, TProjection> WithSelect(Expression<Func<TEntity, TProjection>> select)
        {
            Select = select;
            return this;
        }
    }
}
