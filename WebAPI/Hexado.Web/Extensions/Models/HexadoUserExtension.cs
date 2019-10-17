using Hexado.Db.Entities;
using Hexado.Web.Models;

namespace Hexado.Web.Extensions.Models
{
    public static class HexadoUserExtension
    {
        public static HexadoUser ToHexadoUser(this RegisterUserModel model)
        {
            return new HexadoUser
            {
                Email = model.Email,
                UserName = model.Username,
                Account = new UserAccount()
            };
        }
    }
}