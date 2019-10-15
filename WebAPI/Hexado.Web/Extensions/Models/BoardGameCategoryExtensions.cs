using Hexado.Db.Entities;
using Hexado.Web.Models;

namespace Hexado.Web.Extensions.Models
{
    public static class BoardGameCategoryExtensions
    {
        public static BoardGameCategory ToEntity(this BoardGameCategoryModel model)
        {
            return model.ToEntity(default);
        }

        public static BoardGameCategory ToEntity(this BoardGameCategoryModel model, string? id)
        {
            return new BoardGameCategory
            {
                Id = id,
                Name = model.Name
            };
        }
    }
}