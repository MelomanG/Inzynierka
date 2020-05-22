using System.Collections.Generic;
using Hexado.Db.Entities;

namespace Hexado.Web.Models.Responses
{
    public class PubResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Address Address { get; set; }
        public string ImagePath { get; set; }

        public string AccountId { get; set; }
        public IEnumerable<PubRate> PubRates { get; set; }
        public IEnumerable<PubBoardGame> PubBoardGames { get; set; }

        public bool IsLikedByUser { get; set; }
        public int AmountOfLikes { get; set; }
    }
}
