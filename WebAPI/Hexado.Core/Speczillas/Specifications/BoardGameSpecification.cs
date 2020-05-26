using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Core.Speczillas.Specifications
{
    public interface IBoardGameSpecification : ISpecification<BoardGame>
    {
    }

    public sealed class BoardGameSpecification : Specification<BoardGame>, IBoardGameSpecification
    {
        public BoardGameSpecification()
            : base(bg => bg.Name, false)
        {
            AddInclude(bg => bg.Category);
            AddInclude(bg => bg.LikedBoardGames);
            AddInclude(bg => bg.BoardGameRates);
            AddInclude($"{nameof(BoardGame.BoardGameRates)}.{nameof(BoardGameRate.HexadoUser)}");
        }
    }
}