using System.Collections.Generic;
using System.Linq;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Db.Extensions
{
    public static class PubDtoExtensions
    {
        public static PaginationResult<PubDto> ToDto(this PaginationResult<Pub> entity)
        {
            return new PaginationResult<PubDto>
            {
                Page = entity.Page,
                PageCount = entity.PageCount,
                PageSize = entity.PageSize,
                Results = entity.Results.Select(p => p.ToDto()).ToList(),
                TotalCount = entity.TotalCount
            };
        }

        public static PubDto ToDto(this Pub entity, int amountOfLikes = 0)
        {
            return new PubDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ContactNumber = entity.ContactNumber,
                ContactEmail = entity.ContactEmail,
                Address = entity.Address,
                ImagePath = entity.ImagePath,
                AccountId = entity.AccountId,
                PubRates = entity.PubRates,
                PubBoardGames = entity.PubBoardGames,
                AmountOfLikes = amountOfLikes == 0 ? entity.LikedPubs.Count : amountOfLikes
            };
        }

        public static IEnumerable<PubDto> ToDto(this IEnumerable<Pub> entities, int amountOfLikes = 0)
        {
            return entities.Select(bg => bg.ToDto(amountOfLikes));
        }
    }
}
