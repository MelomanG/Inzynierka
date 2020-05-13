using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IBoardGameService : IBaseService<BoardGame>
    {
    }

    public class BoardGameService : BaseService<BoardGame>, IBoardGameService
    {
        private readonly IBoardGameRepository _boardGameRepository;

        public BoardGameService(IBoardGameRepository boardGameRepository)
            : base(boardGameRepository)
        {
            _boardGameRepository = boardGameRepository;
        }

        public override Task<Maybe<BoardGame>> GetByIdAsync(string id)
        {
            return _boardGameRepository.GetSingleOrMaybeAsync(
                bg => bg.Id == id,
                bg => bg.Category);
        }
    }
}