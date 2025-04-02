using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMB.Abstractions.Shared.Contracts.Paginations.Requests;
using RMB.Abstractions.Shared.Contracts.Paginations.Responses;
using RMB.Core.Paginations.Services;
using System.Linq.Expressions;

namespace RMB.Core.Paginations.Extensions
{
    /// <summary>
    /// Provides extension methods for IQueryable to support paginated queries,
    /// including filtering, projection, grouping, and ordering.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Executes a paginated query with optional filtering, projection, ordering, and grouping.
        /// </summary>
        public static async Task<PaginatedResult<TProjection>> ToPaginatedResultAsync<TEntity, TProjection>(
            this IQueryable<TEntity> source,
            PaginationRequest paginationRequest,
            PaginatedQueryOptions<TEntity, TProjection> options,
            CancellationToken cancellationToken)
            where TEntity : class
        {
            if (options.Include != null)
                source = options.Include(source);

            if (options.Predicate != null)
                source = source.Where(options.Predicate);

            if (options.GroupBy != null && options.SelectGroupBy != null)
                return await ExecuteGroupedPagination(source, paginationRequest, options.GroupBy, options.SelectGroupBy, cancellationToken);

            if (options.OrderBy != null)
                source = options.OrderBy(source);

            IQueryable<TProjection> query = options.Select != null
                ? source.Select(options.Select)
                : (IQueryable<TProjection>)source;

            return await ExecutePagination(query, paginationRequest, cancellationToken);
        }

        /// <summary>
        /// Executes a paginated query using a group by clause and a group projection.
        /// </summary>
        private static async Task<PaginatedResult<TProjection>> ExecuteGroupedPagination<TEntity, TProjection>(
            IQueryable<TEntity> source,
            PaginationRequest pagination,
            Expression<Func<TEntity, object>> groupBy,
            Expression<Func<IGrouping<object, TEntity>, TProjection>> selectGroupBy,
            CancellationToken cancellationToken)
        {
            var groupedQuery = source.GroupBy(groupBy);

            var totalItems = await groupedQuery.CountAsync(cancellationToken);
            var items = await groupedQuery
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(selectGroupBy)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<TProjection>
            {
                Items = items,
                Pagination = BuildMetadata(pagination, totalItems)
            };
        }

        /// <summary>
        /// Executes a regular paginated query with a flat projection.
        /// </summary>
        private static async Task<PaginatedResult<TProjection>> ExecutePagination<TProjection>(
            IQueryable<TProjection> query,
            PaginationRequest pagination,
            CancellationToken cancellationToken)
        {
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<TProjection>
            {
                Items = items,
                Pagination = BuildMetadata(pagination, totalItems)
            };
        }

        /// <summary>
        /// Builds pagination metadata based on request parameters and total item count.
        /// </summary>
        private static PaginationMetadata BuildMetadata(PaginationRequest request, int totalItems)
        {
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            return new PaginationMetadata
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
