namespace Hexado.Web.Models.Responses
{
    public class AddressResponse
    {
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string LocalNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public string PubId { get; set; }
    }
}
