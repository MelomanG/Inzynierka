using System;
using System.Collections.Generic;

namespace Hexado.Web.Models.Responses
{
    public class EventResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public AddressResponse Address { get; set; }
        public string ImagePath { get; set; }
        public bool IsPublic { get; set; }
        public HexadoUserResponse Owner { get; set; }
        public PubResponse Pub { get; set; }
        public BoardGameResponse BoardGame { get; set; }
        public IEnumerable<HexadoUserResponse> Participants { get; set; }
        public bool IsUserEvent { get; set; }
        public bool IsUserParticipant { get; set; }
    }
}