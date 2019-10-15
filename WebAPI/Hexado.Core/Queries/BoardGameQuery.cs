using Microsoft.AspNetCore.Mvc;

namespace Hexado.Core.Queries
{
    public class BoardGameQuery : PaginationQuery
    {
        [FromQuery(Name = "category")]
        public string[] Categories  { get; set; }

        [FromQuery(Name = "min_players")]
        public int? MinPlayers  { get; set; }

        [FromQuery(Name = "max_players")]
        public int? MaxPlayers  { get; set; }

        [FromQuery(Name = "min_age")]
        public int? MinAge  { get; set; }

        [FromQuery(Name = "max_age")]
        public int? MaxAge { get; set; }
    }
}