using Hexado.Db.Entities;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;

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

        public static AddressResponse ToResponse(this Address entity)
        {
            return new AddressResponse
            {
                City = entity.City,
                PostalCode = entity.PostalCode,
                Street = entity.Street,
                BuildingNumber = entity.BuildingNumber,
                LocalNumber = entity.LocalNumber,
                PubId = entity.PubId
            };
        }
    }
}