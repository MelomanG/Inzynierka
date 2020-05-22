using System.Collections.Generic;
using System.Linq;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Db.Extensions
{
    public static class BoardGameDtoExtensions
    {
        public static PaginationResult<BoardGameDto> ToDto(this PaginationResult<BoardGame> entity)
        {
            return new PaginationResult<BoardGameDto>
            {
                Page = entity.Page,
                PageCount = entity.PageCount,
                PageSize = entity.PageSize,
                Results = entity.Results.Select(p => p.ToDto()).ToList(),
                TotalCount = entity.TotalCount
            };
        }

        public static BoardGameDto ToDto(this BoardGame entity, int amountOfLikes = 0)
        {
            return new BoardGameDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                MinPlayers = entity.MinPlayers,
                MaxPlayers = entity.MaxPlayers,
                FromAge = entity.FromAge,
                ImagePath = entity.ImagePath,
                CategoryId = entity.CategoryId,
                Category = entity.Category.ToDto(),
                BoardGameRates = entity.BoardGameRates,
                AmountOfLikes = amountOfLikes == 0 ? entity.LikedBoardGames.Count : amountOfLikes
            };
        }

        public static IEnumerable<BoardGameDto> ToDto(this IEnumerable<BoardGame> entities, int amountOfLikes = 0)
        {
            return entities.Select(bg => bg.ToDto(amountOfLikes));
        }
    }
}
