using System.Linq;
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
    }
}