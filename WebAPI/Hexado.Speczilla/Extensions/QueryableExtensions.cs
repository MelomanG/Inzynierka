using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Speczilla.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query,
            ISpecification<T> specification) where T : class
        {
            var specQuery = specification.Where != null
                ? query.Where(specification.Where)
                : query;

            specQuery = specification.Includes.Aggregate(specQuery,
                (current, include) => current.Include(include));

            specQuery = specification.IncludeStrings.Aggregate(specQuery,
                (current, include) => current.Include(include));

            if (specification.OrderBy != null)
                specQuery = specification.IsOrderDescending
                    ? specQuery.OrderByDescending(specification.OrderBy)
                    : specQuery.OrderBy(specification.OrderBy);

            return specQuery;
        }

        public static async Task<PaginationResult<T>> AsPaginationResultAsync<T>(this IQueryable<T> query, ISpecification<T> specification) where T: class
        {
            var specQuery = query.ApplySpecification(specification);

            var totalCount = specQuery.Count();
            var skip = specification.Page <= 0
                                    ? 0
                                    : (specification.Page - 1) * specification.PageSize;

            specQuery = specQuery
                .Skip(skip)
                .Take(specification.PageSize);

            var listAsync = await specQuery.ToListAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / specification.PageSize);

            return new PaginationResult<T>
            {
                Page = specification.Page,
                PageSize = specification.PageSize,
                PageCount = pageCount,
                TotalCount = totalCount,
                Results = listAsync
            };
        }
    }
}