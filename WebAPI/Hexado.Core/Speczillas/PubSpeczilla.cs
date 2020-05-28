using System.Linq;
using Hexado.Core.Queries;
using Hexado.Core.Speczillas.Specifications;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Speczilla.Constants;

namespace Hexado.Core.Speczillas
{
    public interface IPubSpeczilla
    {
        ISpecification<Pub> GetSpecification(PubQuery query);
    }

    public class PubSpeczilla : IPubSpeczilla
    {
        public ISpecification<Pub> GetSpecification(PubQuery query)
        {
            var specification = new PubSpecification();

            specification.SetPage(query.Page);
            specification.SetPageSize(query.PageSize);

            specification
                .AddInclude(bg => bg.PubBoardGames)
                .AddInclude($"{nameof(Pub.PubBoardGames)}.{nameof(PubBoardGame.BoardGame)}")
                .AddInclude(pub => pub.Address)
                .AddInclude(pub => pub.LikedPubs);

            if (!string.IsNullOrWhiteSpace(query.BoardGameId))
            {
                specification
                    .AndAlso(pub=> pub.PubBoardGames
                        .Any(game => game.BoardGameId == query.BoardGameId));
            }

            if (!string.IsNullOrWhiteSpace(query.City))
            {
                specification
                    .AndAlso(pub => pub.Address.City == query.City);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                specification
                    .AndAlso(pub => pub.Name.Contains(query.Search));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
                SetOrderBy(specification, query.OrderBy);

            return specification;
        }

        private static void SetOrderBy(Specification<Pub> specification, string queryOrderBy)
        {
            var sortParam = queryOrderBy.Split(' ').ToList();
            if (sortParam.Count > 2)
                return;

            var isDescending = sortParam.Contains(QueryParamKey.Desc);
            switch (sortParam[0])
            {
                case "rate":
                    specification.SetOrderBy(p => p.PubRates.Sum(pr => pr.UserRate) / p.PubRates.Count, isDescending);
                    break;

                case "name":
                    specification.SetOrderBy(bg => bg.Name, isDescending);
                    break;

                case "like":
                    specification.SetOrderBy(bg => bg.LikedPubs.Count, isDescending);
                    break;

                default:
                    specification.SetOrderBy(bg => bg.Name);
                    break;
            }
        }
    }
}