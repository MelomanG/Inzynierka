using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IRateService
    {
        Task<Maybe<BoardGame>> RateBoardGame(BoardGameRate rate);
        Task<Maybe<BoardGameRate>> UpdateBoardGameRate(BoardGameRate rate);
        Task<Maybe<BoardGameRate>> DeleteBoardGameRate(string rateId);

        Task<Maybe<Pub>> RatePub(PubRate rate);
        Task<Maybe<PubRate>> UpdatePubRate(PubRate rate);
        Task<Maybe<PubRate>> DeletePubRate(string rateId);
    }

    public class RateService : IRateService
    {
        private readonly IRepository<BoardGameRate> _boardGameRateRepository;
        private readonly IRepository<PubRate> _pubRateRepository;
        private readonly IBoardGameRepository _boardGameRepository;
        private readonly IPubRepository _pubRepository;

        public RateService(
            IRepository<BoardGameRate> boardGameRateRepository,
            IRepository<PubRate> pubRateRepository,
            IBoardGameRepository boardGameRepository,
            IPubRepository pubRepository)
        {
            _boardGameRateRepository = boardGameRateRepository;
            _pubRateRepository = pubRateRepository;
            _boardGameRepository = boardGameRepository;
            _pubRepository = pubRepository;
        }

        public async Task<Maybe<BoardGame>> RateBoardGame(BoardGameRate rate)
        {
            var boardGame = await _boardGameRepository.GetAsync(rate.BoardGameId);
            if (!boardGame.HasValue)
                return boardGame;

            boardGame.Value.BoardGameRates.Add(rate);
            return await _boardGameRepository.UpdateAsync(boardGame.Value);
        }

        public Task<Maybe<BoardGameRate>> UpdateBoardGameRate(BoardGameRate rate)
        {
            return _boardGameRateRepository.UpdateAsync(rate);
        }

        public Task<Maybe<BoardGameRate>> DeleteBoardGameRate(string rateId)
        {
            return _boardGameRateRepository.DeleteByIdAsync(rateId);
        }

        public async Task<Maybe<Pub>> RatePub(PubRate rate)
        {
            var pub = await _pubRepository.GetAsync(rate.PubId);
            if (!pub.HasValue)
                return pub;

            pub.Value.PubRates.Add(rate);
            return await _pubRepository.UpdateAsync(pub.Value);
        }

        public Task<Maybe<PubRate>> UpdatePubRate(PubRate rate)
        {
            return _pubRateRepository.UpdateAsync(rate);
        }

        public Task<Maybe<PubRate>> DeletePubRate(string rateId)
        {
            return _pubRateRepository.DeleteByIdAsync(rateId);
        }
    }
}