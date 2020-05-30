using System.Collections.Generic;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;
using System.Linq;
using Hexado.Db.Dtos;

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
                Description = model.Description,
                Address = model.Address.ToEntity(pubId),
                ImagePath = model.ImagePath ?? string.Empty
            };
        }

        public static PaginationResult<PubResponse> ToResponse(this PaginationResult<Pub> entity)
        {
            return new PaginationResult<PubResponse>
            {
                Page = entity.Page,
                PageCount = entity.PageCount,
                PageSize = entity.PageSize,
                Results = entity.Results.Select(p => p.ToResponse()).ToList(),
                TotalCount = entity.TotalCount
            };
        }

        public static PubResponse ToResponse(this Pub entity, bool isLikedByUser = false, bool isUserPub = false)
        {
            return new PubResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Address = entity.Address?.ToResponse(),
                ImagePath = entity.ImagePath,
                AccountId = entity.AccountId,
                Rates = entity.PubRates.Select(r => r.ToRateResponse()),
                PubBoardGames = entity.PubBoardGames?.Select(pbg => pbg?.BoardGame?.ToResponse()),
                IsLikedByUser = isLikedByUser,
                AmountOfLikes = entity.LikedPubs.Count
            };
        }

        public static IEnumerable<PubResponse> ToResponse(this IEnumerable<Pub> entities, bool isLikedByUser = false, bool isUserPub = false)
        {
            return entities.Select(bg => bg.ToResponse(isLikedByUser, isUserPub));
        }

        public static PaginationResult<PubResponse> ToResponse(this PaginationResult<PubDto> dto)
        {
            return new PaginationResult<PubResponse>
            {
                Page = dto.Page,
                PageCount = dto.PageCount,
                PageSize = dto.PageSize,
                Results = dto.Results.Select(p => p.ToResponse()).ToList(),
                TotalCount = dto.TotalCount
            };
        }

        public static PubResponse ToResponse(this PubDto dto, bool isLikedByUser = false, bool isUserPub = false)
        {
            return new PubResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address?.ToResponse(),
                ImagePath = dto.ImagePath,
                AccountId = dto.AccountId,
                Rates = dto.PubRates.Select(r => r.ToRateResponse()),
                PubBoardGames = dto.PubBoardGames.Select(pbg => pbg.BoardGame.ToResponse()),
                IsLikedByUser = isLikedByUser,
                IsUserPub = isUserPub,
                AmountOfLikes = dto.AmountOfLikes
            };
        }

        public static IEnumerable<PubResponse> ToResponse(this IEnumerable<PubDto> dtos, bool isLikedByUser = false)
        {
            return dtos.Select(bg => bg.ToResponse(isLikedByUser));
        }
    }
}