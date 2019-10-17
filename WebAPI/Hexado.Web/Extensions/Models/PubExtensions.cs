using Hexado.Db.Entities;
using Hexado.Web.Models;

namespace Hexado.Web.Extensions.Models
{
    public static class PubExtensions
    {
        public static Pub ToEntity(this PubModel model, string accountId)
        {
            return model.ToEntity(accountId, default);
        }

        public static Pub ToEntity(this PubModel model, string accountId, string? pubId)
        {
            return new Pub
            {
                Id = pubId,
                AccountId = accountId,
                Name = model.Name,
                Description = model.Description
            };
        }
    }
}