using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hexado.Speczilla
{
    public interface ISpecification<T>
    {
        List<Expression<Func<T, bool>>> Wheres { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        int Page { get; }
        int PageSize { get; }
    }

    public class Specification<T> : ISpecification<T>
    {
        public List<Expression<Func<T, bool>>> Wheres { get; private set;  } = new List<Expression<Func<T, bool>>>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public int Page { get; private set; } = 1;
        public int PageSize { get; private set; } = 20;

        public Specification<T> SetPage(int page)
        {
            Page = page;
            return this;
        }

        public Specification<T> SetPageSize(int pageSize)
        {
            PageSize = pageSize;
            return this;
        }

        public Specification<T> SetOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
            return this;
        }

        public Specification<T> SetOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
            return this;
        }

        public Specification<T> SetWhere(Expression<Func<T, bool>> where)
        {
            Wheres.Add(where);
            return this;
        }
    }
}