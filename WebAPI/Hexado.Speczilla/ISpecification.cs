using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hexado.Speczilla
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Where { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        bool IsOrderDescending { get; }
        int Page { get; }
        int PageSize { get; }
    }
}