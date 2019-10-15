using Microsoft.AspNetCore.Mvc;

namespace Hexado.Core.Queries
{
    public abstract class PaginationQuery
    {
        [FromQuery(Name = "page")]
        public int? Page { get; set; }

        [FromQuery(Name = "page_size")]
        public int? PageSize { get; set; }

        [FromQuery(Name = "sort")]
        public string OrderBy { get; set; }
    }
}