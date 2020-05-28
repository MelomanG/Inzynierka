using System.Collections.Generic;
using System.Linq;
using Hexado.Db.Entities;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;

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

        public static HexadoUserResponse ToResponse(this HexadoUser entity)
        {
            return new HexadoUserResponse
            {
                Id = entity.Id,
                UserName = entity.UserName
            };
        }

        public static IEnumerable<HexadoUserResponse> ToResponse(this IEnumerable<HexadoUser> entities)
        {
            return entities.Select(e => e.ToResponse());
        }
    }
}