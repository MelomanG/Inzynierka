using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IBoardGameService : IBaseService<BoardGame>
    {
    }

    public class BoardGameService : BaseService<BoardGame>, IBoardGameService
    {
        public BoardGameService(IBoardGameRepository boardGameRepository)
            : base(boardGameRepository)
        {
        }
    }
}