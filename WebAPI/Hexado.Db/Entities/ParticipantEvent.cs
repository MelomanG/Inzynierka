namespace Hexado.Db.Entities
{
    public class ParticipantEvent: BaseEntity
    {
        public string ParticipantId { get; set; }
        public virtual HexadoUser Participant { get; set; }

        public string EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}