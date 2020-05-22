using Hexado.Core.Models;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;

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

        public static BoardGameCategoryResponse ToResponse(this BoardGameCategory entity)
        {
            return new BoardGameCategoryResponse
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static BoardGameCategoryResponse ToResponse(this BoardGameCategoryDto dto)
        {
            return new BoardGameCategoryResponse
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}