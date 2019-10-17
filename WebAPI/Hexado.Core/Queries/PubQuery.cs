using Microsoft.AspNetCore.Mvc;

namespace Hexado.Core.Queries
{
    public class PubQuery : PaginationQuery
    {
        [FromQuery(Name = "board_game_id")]
        public string BoardGameId { get; set; }

        [FromQuery(Name = "city")]
        public string City { get; set; }
    }
}