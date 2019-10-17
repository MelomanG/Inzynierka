using System.Linq;
using Hexado.Core.Queries;
using Hexado.Core.Speczillas.Specifications;
using Hexado.Db.Entities;
using Hexado.Speczilla;

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

            if (!string.IsNullOrWhiteSpace(query.BoardGameId))
            {
                specification
                    .AddInclude(bg => bg.PubBoardGames)
                    .AddInclude($"{nameof(Pub.PubBoardGames)}.{nameof(PubBoardGame.BoardGame)}")
                    .AndAlso(pub=> pub.PubBoardGames
                        .Any(game => game.BoardGameId == query.BoardGameId));
            }

            if (string.IsNullOrWhiteSpace(query.City))
            {
                specification
                    .AddInclude(bg => bg.Address)
                    .AndAlso(pub => pub.Address.City == query.City);
            }

            return specification;
        }
    }
}