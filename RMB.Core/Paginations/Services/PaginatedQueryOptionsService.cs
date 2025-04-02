using System.Linq.Expressions;

namespace RMB.Core.Paginations.Services
{
    /// <summary>
    /// Encapsulates all optional configuration options for building a paginated query.
    /// Used in conjunction with paginated query builders and extension methods to apply filters, 
    /// includes, ordering, grouping, and projections to queries.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <typeparam name="TProjection">The type of the final projected result.</typeparam>
    public class PaginatedQueryOptionsService<TEntity, TProjection>
    {
        /// <summary>
        /// Filter condition to be applied to the query.
        /// </summary>
        public Expression<Func<TEntity, bool>>? Predicate { get; set; }

        /// <summary>
        /// Includes related entities for eager loading.
        /// </summary>
        public Func<IQueryable<TEntity>, IQueryable<TEntity>>? Include { get; set; }

        /// <summary>
        /// Specifies ordering to apply to the query result.
        /// </summary>
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; set; }

        /// <summary>
        /// Groups the query results based on the provided expression.
        /// </summary>
        public Expression<Func<TEntity, object>>? GroupBy { get; set; }

        /// <summary>
        /// Projects grouped data to a target result shape.
        /// Only applied if GroupBy is also defined.
        /// </summary>
        public Expression<Func<IGrouping<object, TEntity>, TProjection>>? SelectGroupBy { get; set; }

        /// <summary>
        /// Projects flat query results to the final result type.
        /// Used when grouping is not applied.
        /// </summary>
        public Expression<Func<TEntity, TProjection>>? Select { get; set; }
    }
}
