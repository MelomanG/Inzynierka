using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IBoardGameService : IBaseService<BoardGame>
    {
        Task<Maybe<BoardGame>> SetImagePath(string id, string imagePath);
        Task ClearAsync();
        Task<Maybe<BoardGame>> LikeBoardGame(string boardGameId, HexadoUser user);
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
                $"{nameof(BoardGame.Category)}",
                $"{nameof(BoardGame.LikedBoardGames)}",
                $"{nameof(BoardGame.BoardGameRates)}.{nameof(BoardGameRate.HexadoUser)}");
        }

        public async Task<Maybe<BoardGame>> SetImagePath(string id, string imagePath)
        {
            var boardGame = await _boardGameRepository.GetAsync(id);
            if (!boardGame.HasValue)
                return boardGame;

            boardGame.Value.ImagePath = imagePath;
            return await _boardGameRepository.UpdateAsync(boardGame.Value);
        }

        public async Task<Maybe<BoardGame>> LikeBoardGame(string boardGameId, HexadoUser user)
        {
            var boardGame = await _boardGameRepository.GetAsync(boardGameId);
            if (!boardGame.HasValue)
                return boardGame;
            boardGame.Value.LikedBoardGames.Add(
                new LikedBoardGame
                {
                    HexadoUserId = user.Id,
                    HexadoUser = user
                });
            return await _boardGameRepository.UpdateAsync(boardGame.Value);
        }
    }
}