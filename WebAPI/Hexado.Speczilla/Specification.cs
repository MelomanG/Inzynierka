using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hexado.Speczilla.Constants;

namespace Hexado.Speczilla
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Where { get; private set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public int Page { get; private set; } = DefaultValues.Page;
        public int PageSize { get; private set; } = DefaultValues.PageSize;
        public bool IsOrderDescending { get; private set; }

        protected Specification(Expression<Func<T, object>> defaultOrderBy, bool isOrderDescending)
        {
            SetOrderBy(defaultOrderBy, isOrderDescending);
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public Specification<T> SetPage(int? page)
        {
            Page = page ?? Page;
            return this;
        }

        public Specification<T> SetPageSize(int? pageSize)
        {
            PageSize = pageSize ?? PageSize;
            return this;
        }

        public Specification<T> SetOrderBy(Expression<Func<T, object>> orderBy, bool isDescending = false)
        {
            OrderBy = orderBy;
            IsOrderDescending = isDescending;
            return this;
        }

        public Specification<T> OrElse(Expression<Func<T, bool>> orWhere)
        {
            //TODO Refactor
            if (Where == null)
            {
                Where = orWhere;
                return this;
            }

            var p = Where.Parameters[0];
            var visitor = new SubstExpressionVisitor
            {
                Subst = {[orWhere.Parameters[0]] = p}
            };

            Expression body = Expression.OrElse(Where.Body, visitor.Visit(orWhere.Body) ?? throw new InvalidOperationException());
            Where = Expression.Lambda<Func<T, bool>>(body, p);

            return this;
        }

        public Specification<T> AndAlso(Expression<Func<T, bool>> andWhere)
        {
            if (Where == null)
            {
                Where = andWhere;
                return this;
            }

            var p = Where.Parameters[0];
            var visitor = new SubstExpressionVisitor
            {
                Subst = {[andWhere.Parameters[0]] = p}
            };

            Expression body = Expression.AndAlso(Where.Body, visitor.Visit(andWhere.Body) ?? throw new InvalidOperationException());
            Where = Expression.Lambda<Func<T, bool>>(body, p);

            return this;
        }
    }
}