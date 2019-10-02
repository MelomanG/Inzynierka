using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Entities
{
    public class HexadoUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}