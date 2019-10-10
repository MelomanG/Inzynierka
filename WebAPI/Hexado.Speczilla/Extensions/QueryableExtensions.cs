﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Speczilla.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginationResult<T>> AsPaginationResultAsync<T>(this IQueryable<T> query, ISpecification<T> specification)
        {
            var specQuery = specification.Wheres
                .Aggregate(query,
                    (s, w) => s.Where(w));

            if (specification.OrderBy != null)
                specQuery = specification.IsOrderDescending
                    ? specQuery.OrderByDescending(specification.OrderBy)
                    : specQuery.OrderBy(specification.OrderBy);

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