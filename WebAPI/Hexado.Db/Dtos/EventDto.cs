using System;
using System.Collections.Generic;
using Hexado.Db.Entities;

namespace Hexado.Db.Dtos
{
    public class EventDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public EventAddress Address { get; set; }
        public string ImagePath { get; set; }
        public bool IsPublic { get; set; }
        public HexadoUser Owner { get; set; }
        public Pub? Pub { get; set; }
        public BoardGame BoardGame { get; set; }
        public IEnumerable<ParticipantEvent> Participants { get; set; }
        public bool IsUserEvent { get; set; }
        public bool IsUserParticipant { get; set; }
    }
}