using System;
using Hexado.Db.Entities;
using Hexado.Web.Models;

namespace Hexado.Web.Extensions.Models
{
    public static class RateExtension
    {
        public static Rate ToEntity(this RateModel model, string? ownerEmail)
        {
            return model.ToEntity(ownerEmail, default);
        }

        public static Rate ToEntity(this RateModel model, string? ownerEmail, string? id)
        {
            return new Rate
            {
                Id = id,
                OwnerEmail = ownerEmail ?? throw new ArgumentNullException($"{nameof(ownerEmail)}"),
                UserRate = model.UserRate,
                Comment = model.Comment
            };
        }
    }
}
