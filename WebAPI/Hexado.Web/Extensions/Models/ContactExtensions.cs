using Hexado.Db.Entities;
using Hexado.Web.Models.Responses;
using System.Collections.Generic;
using System.Linq;

namespace Hexado.Web.Extensions.Models
{
    public static class ContactExtensions
    {
        public static ContactResponse ToResponse(this Contact entity)
        {
            return new ContactResponse
            {
                Id = entity.Id,
                ContactUserId = entity.ContactHexadoUser.Id,
                ContactUserName = entity.ContactHexadoUser.UserName,
            };
        }

        public static IEnumerable<ContactResponse> ToResponse(this IEnumerable<Contact> entities)
        {
            return entities.Select(c => c.ToResponse());
        }
    }
}