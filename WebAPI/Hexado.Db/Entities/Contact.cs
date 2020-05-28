namespace Hexado.Db.Entities
{
#nullable disable
    public class Contact: BaseEntity
    {
        public string ContactHexadoUserId { get; set; }
        public virtual HexadoUser ContactHexadoUser { get; set; }
        public string HexadoUserId { get; set; }
        public virtual HexadoUser HexadoUser { get; set; }
    }
#nullable enable
}
