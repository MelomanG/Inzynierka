﻿using System.Linq;
using Hexado.Core.Queries;
using Hexado.Core.Speczillas.Specifications;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Speczilla.Constants;

namespace Hexado.Core.Speczillas
{
    public interface IBoardGameSpeczilla
    {
        IBoardGameSpecification GetSpecification(BoardGameQuery query);
        IBoardGameSpecification GetSpecification(BoardGameQuery query, string pubId);
    }

    public class BoardGameSpeczilla : IBoardGameSpeczilla
    {
        public IBoardGameSpecification GetSpecification(BoardGameQuery query, string pubId)
        {
            var specification = GetBaseSpecification(query);
            specification.AndAlso(game => game.PubBoardGames.Any(boardGame => boardGame.PubId == pubId));
            
            return specification;
        }

        public IBoardGameSpecification GetSpecification(BoardGameQuery query)
        {
            return GetBaseSpecification(query);
        }

        private static BoardGameSpecification GetBaseSpecification(BoardGameQuery query)
        {
            var specification = new BoardGameSpecification();

            specification.SetPage(query.Page);
            specification.SetPageSize(query.PageSize);

            foreach (var categoryId in query.Categories ?? Enumerable.Empty<string>())
            {
                specification.OrElse(bg => bg.CategoryId == categoryId);
            }

            if (query.MinPlayers.HasValue)
                specification.AndAlso(bg => bg.MinPlayers <= query.MinPlayers && bg.MaxPlayers >= query.MinPlayers);

            if (query.MaxPlayers.HasValue)
                specification.AndAlso(bg => bg.MinPlayers <= query.MaxPlayers && bg.MaxPlayers >= query.MaxPlayers);

            if (query.MinAge.HasValue)
                specification.AndAlso(bg => bg.FromAge >= query.MinAge);

            if (!string.IsNullOrWhiteSpace(query.Search))
                specification
                    .AndAlso(pub => pub.Name.Contains(query.Search));

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
                SetOrderBy(specification, query.OrderBy);

            return specification;
        }

        private static void SetOrderBy(Specification<BoardGame> specification, string queryOrderBy)
        {
            var sortParam = queryOrderBy.Split(' ').ToList();
            if (sortParam.Count > 2)
                return;

            var isDescending = sortParam.Contains(QueryParamKey.Desc);
            switch (sortParam[0])
            {
                case "rate":
                    specification.SetOrderBy(bg => bg.BoardGameRates.Sum(bgr => bgr.UserRate) / bg.BoardGameRates.Count, isDescending);
                    break;

                case "name":
                    specification.SetOrderBy(bg => bg.Name, isDescending);
                    break;

                case "like":
                    specification.SetOrderBy(bg => bg.LikedBoardGames.Count, isDescending);
                    break;

                default:
                    specification.SetOrderBy(bg => bg.Name);
                    break;
            }
        }
    }
}