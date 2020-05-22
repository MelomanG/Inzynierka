using Hexado.Db.Dtos;
using Hexado.Db.Entities;

namespace Hexado.Db.Extensions
{
    public static class BoardGameCategoryDtoExtensions
    {
        public static BoardGameCategoryDto ToDto(this BoardGameCategory entity)
        {
            return new BoardGameCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
