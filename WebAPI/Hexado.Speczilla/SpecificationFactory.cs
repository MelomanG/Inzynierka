using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Hexado.Speczilla.Constants;
using Microsoft.AspNetCore.Http;

namespace Hexado.Speczilla
{
    public interface ISpecificationFactory<T>
    {
        Specification<T> CreateSpecification(IQueryCollection queryCollection);
    }

    public class SpecificationFactory<T> : ISpecificationFactory<T>
    {
        public Specification<T> CreateSpecification(IQueryCollection queryCollection)
        {
            var specification = new Specification<T>();

            SetPaging(queryCollection, specification);
            SetSorting(queryCollection, specification);
            SetFiltering(queryCollection, specification);
            return specification;
        }

        private static void SetPaging(IQueryCollection queryCollection, Specification<T> specification)
        {
            if (queryCollection.TryGetValue(QueryParamKey.Page, out var pageStringValues))
            {
                var pageParam = pageStringValues.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(pageParam) && int.TryParse(pageParam, out var parsedPage))
                {
                    if(parsedPage > 0)
                        specification.SetPage(parsedPage);
                }
            }

            if (queryCollection.TryGetValue(QueryParamKey.PageSize, out var pageSizeStringValues))
            {
                var pageSizeParam = pageSizeStringValues.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(pageSizeParam) && int.TryParse(pageSizeParam, out var parsedPageSize))
                {
                    if(parsedPageSize > 0 )
                        specification.SetPageSize(parsedPageSize);
                }
            }
        }

        private static void SetSorting(IQueryCollection queryCollection, Specification<T> specification)
        {
            if (!queryCollection.TryGetValue(QueryParamKey.Sort, out var sortStringValues))
                return;

            var sortParam = sortStringValues.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sortParam))
                return;

            var sortParamValues = sortParam.Split(' ').ToList();
            if (sortParamValues.Count > 2)
                return;

            var sortPropertyNameParam = sortParamValues[0];
            var camelCaseParam = CapitalizeFirstLetter(sortPropertyNameParam);
            var propertyInfo = typeof(T).GetProperty(camelCaseParam) ?? typeof(T).GetProperty(sortPropertyNameParam);
            if (propertyInfo == null)
                return;


            var orderBy = CreateExpression(typeof(T), sortPropertyNameParam);
            if(orderBy != null)
                specification.SetOrderBy(orderBy,
                    sortParamValues.Contains(QueryParamKey.Desc));
        }

        private static string CapitalizeFirstLetter(string propertyName)
        {
            return propertyName[0].ToString().ToUpperInvariant() + propertyName.Substring(1);
        }

        private static Expression<Func<T, object>> CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type);
            Expression body = Expression.PropertyOrField(param, CapitalizeFirstLetter(propertyName)) ??
                              Expression.PropertyOrField(param, propertyName);
            body = Expression.Convert(body, typeof(object));
            return Expression.Lambda<Func<T, object>>(body, param);
        }

        private static void SetFiltering(IQueryCollection queryCollection, Specification<T> specification)
        {
            if (!queryCollection.TryGetValue(QueryParamKey.Filter, out var filterStringValues))
                return;

            if (!filterStringValues.Any())
                return;

            foreach (var filter in filterStringValues.Where(filter => !string.IsNullOrWhiteSpace(filter)))
            {
                var filterWithoutWhiteSpace = Regex.Replace(filter, @"\s+", "");
                var filterType = filterWithoutWhiteSpace.Substring(0, filterWithoutWhiteSpace.IndexOf("(", StringComparison.Ordinal));

                var filterNotSplitParams = Regex.Match(filterWithoutWhiteSpace, "(?<=\\().+?(?=\\))").Value;
                var filterParams = filterNotSplitParams.Split(",");

                if (filterParams.Length == 2)
                {
                    var whereMethod = GenerateFilterMethod(filterType, filterParams);
                    if(whereMethod != null)
                        specification.SetWhere(whereMethod);
                }
            }
        }

        private static Expression<Func<T, bool>> GenerateFilterMethod(string filterType, IReadOnlyList<string> filterParams)
        {
            var param = Expression.Parameter(typeof(T));
            var property = Expression.PropertyOrField(param, CapitalizeFirstLetter(filterParams[0])) ??
                              Expression.PropertyOrField(param, filterParams[0]);

            var converter = TypeDescriptor.GetConverter(property.Type);
            if (!converter.CanConvertFrom(filterParams[1].GetType()))
                return null;

            var convertedValue = converter.ConvertFromInvariantString(filterParams[1]);
            Expression target = Expression.Constant(convertedValue);
            
            switch (filterType)
            {
                case QueryParamKey.AreEqual:
                    if (property.Type == typeof(string))
                    {
                        var containsMethod =
                            typeof(string).GetMethod("Equals", new[] { typeof(string) });
                        if (containsMethod != null)
                        {
                            var call = Expression.Call(property, containsMethod, target);
                            return Expression.Lambda<Func<T, bool>>(call, param);
                        }
                    }
                    else
                    {
                        var equalBody = Expression.Equal(property, target);
                        return Expression.Lambda<Func<T, bool>>(equalBody, param);
                    }

                    break;

                case QueryParamKey.IsGreater:
                    var greaterThanBody = Expression.GreaterThan(property, target);
                    return Expression.Lambda<Func<T, bool>>(greaterThanBody, param);

                case QueryParamKey.IsLess:
                    var lessThanBody = Expression.LessThan(property, target);
                    return Expression.Lambda<Func<T, bool>>(lessThanBody, param);

                case QueryParamKey.Contains:
                    if (property.Type == typeof(string))
                    {
                        var containsMethod =
                            typeof(string).GetMethod("Contains", new[] {typeof(string)});
                        if (containsMethod != null)
                        {
                            var call = Expression.Call(property, containsMethod, target);
                            return Expression.Lambda<Func<T, bool>>(call, param);
                        }
                    }
                    break;
            }

            return null;
        }

        private static Expression<Func<T, bool>> CreatePredicateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type);
            Expression body = Expression.PropertyOrField(param, CapitalizeFirstLetter(propertyName)) ??
                              Expression.PropertyOrField(param, propertyName);
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}