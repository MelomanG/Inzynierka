using System.Collections.Generic;

namespace Hexado.Speczilla
{
    public class PaginationResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}