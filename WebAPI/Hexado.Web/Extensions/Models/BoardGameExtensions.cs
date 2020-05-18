using System;
using Hexado.Db.Entities;
using Hexado.Web.Models;

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
    }
}