using System;
using System.Collections.Generic;
using System.Linq;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;

namespace Hexado.Web.Extensions.Models
{
    public static class BoardGameExtensions
    {
        public static BoardGame ToEntity(this BoardGameModel model)
        {
            return model.ToEntity(default);
        }

        public static BoardGame ToEntity(this BoardGameModel model, string? id)
        {
            return new BoardGame
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                MinPlayers = model.MinPlayers ?? throw new ArgumentNullException($"{nameof(BoardGameModel)}.{nameof(BoardGameModel.MinPlayers)}"),
                MaxPlayers = model.MaxPlayers ?? throw new ArgumentNullException($"{nameof(BoardGameModel)}.{nameof(BoardGameModel.MaxPlayers)}"),
                FromAge = model.FromAge ?? throw new ArgumentNullException($"{nameof(BoardGameModel)}.{nameof(BoardGameModel.FromAge)}"),
                CategoryId = model.CategoryId,
                ImagePath = model.ImagePath ?? string.Empty
            };
        }

        public static PaginationResult<BoardGameResponse> ToResponse(this PaginationResult<BoardGame> entity)
        {
            return new PaginationResult<BoardGameResponse>
            {
                Page = entity.Page,
                PageCount = entity.PageCount,
                PageSize = entity.PageSize,
                Results = entity.Results.Select(p => p.ToResponse()).ToList(),
                TotalCount = entity.TotalCount
            };
        }

        public static BoardGameResponse ToResponse(this BoardGame entity, bool isLikedByUser = false)
        {
            return new BoardGameResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                MinPlayers = entity.MinPlayers,
                MaxPlayers = entity.MaxPlayers,
                FromAge = entity.FromAge,
                ImagePath = entity.ImagePath,
                CategoryId = entity.CategoryId,
                Category = entity.Category?.ToResponse(),
                Rates = entity.BoardGameRates.Select(r => r.ToRateResponse()),
                IsLikedByUser = isLikedByUser,
                AmountOfLikes = entity.LikedBoardGames.Count
            };
        }

        public static IEnumerable<BoardGameResponse> ToResponse(this IEnumerable<BoardGame> entities, bool isLikedByUser = false)
        {
            return entities.Select(bg => bg.ToResponse(isLikedByUser));
        }

        public static PaginationResult<BoardGameResponse> ToResponse(this PaginationResult<BoardGameDto> dto)
        {
            return new PaginationResult<BoardGameResponse>
            {
                Page = dto.Page,
                PageCount = dto.PageCount,
                PageSize = dto.PageSize,
                Results = dto.Results.Select(p => p.ToResponse()).ToList(),
                TotalCount = dto.TotalCount
            };
        }

        public static BoardGameResponse ToResponse(this BoardGameDto dto, bool isLikedByUser = false)
        {
            return new BoardGameResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                MinPlayers = dto.MinPlayers,
                MaxPlayers = dto.MaxPlayers,
                FromAge = dto.FromAge,
                ImagePath = dto.ImagePath,
                CategoryId = dto.CategoryId,
                Category = dto.Category.ToResponse(),
                Rates = dto.BoardGameRates.Select(r => r.ToRateResponse()),
                IsLikedByUser = isLikedByUser,
                AmountOfLikes = dto.AmountOfLikes
            };
        }

        public static IEnumerable<BoardGameResponse> ToResponse(this IEnumerable<BoardGameDto> dtos, bool isLikedByUser = false)
        {
            return dtos.Select(bg => bg.ToResponse(isLikedByUser));
        }
    }
}