using System.Collections.Generic;
using Hexado.Db.Entities;

namespace Hexado.Web.Models.Responses
{
    public class PubResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }

        public AddressResponse Address { get; set; }
        public string ImagePath { get; set; }

        public string AccountId { get; set; }
        public IEnumerable<RateResponse> Rates { get; set; }
        public IEnumerable<BoardGameResponse> PubBoardGames { get; set; }

        public bool IsLikedByUser { get; set; }
        public bool IsUserPub { get; set; }
        public int AmountOfLikes { get; set; }
    }
}
