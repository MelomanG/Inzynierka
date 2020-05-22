using Hexado.Db.Entities;
using Hexado.Web.Models;

namespace Hexado.Web.Extensions.Models
{
    public static class AddressExtensions
    {
        public static Address ToEntity(this AddressModel model)
        {
            return ToEntity(model, default);
        }

        public static Address ToEntity(this AddressModel model, string? pubId)
        {
            return new Address
            {
                PubId = pubId,
                Street = model.Street,
                BuildingNumber = model.BuildingNumber,
                LocalNumber = model.LocalNumber,
                PostalCode = model.PostalCode,
                City = model.City
            };
        }
    }
}