using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Services.Exceptions;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IPubService : IBaseService<Pub>
    {
        Task<Maybe<Pub>> DeleteByIdAsync(string pubId, string userAccountId);
        Task<Maybe<Pub>> AddBoardGames(string pubId, string accountId, string boardGameId);
        Task<Maybe<Pub>> DeleteBoardGames(string pubId, string accountId, string boardGameId);
        Task<Maybe<Pub>> SetImagePath(string id, string imagePath);
        Task<Maybe<Pub>> LikePub(string boardGameId, HexadoUser user);
        Task ClearAsync();
        Maybe<IEnumerable<string>> GetCities();
    }

    public class PubService : BaseService<Pub>, IPubService
    {
        private readonly IPubRepository _pubRepository;
        private readonly IRepository<PubBoardGame> _pubBoardGameRepository;

        public PubService(
            IPubRepository pubRepository,
            IRepository<PubBoardGame> pubBoardGameRepository)
            : base(pubRepository)
        {
            _pubRepository = pubRepository;
            _pubBoardGameRepository = pubBoardGameRepository;
        }

        public override async Task<Maybe<Pub>> GetByIdAsync(string id)
        {
            return await _pubRepository.GetSingleOrMaybeAsync(
                p => p.Id == id,
                $"{nameof(Pub.Address)}",
                $"{nameof(Pub.LikedPubs)}",
                $"{nameof(Pub.PubRates)}.{nameof(PubRate.HexadoUser)}",
                $"{nameof(Pub.PubBoardGames)}.{nameof(PubBoardGame.BoardGame)}",
                $"{nameof(Pub.PubBoardGames)}.{nameof(PubBoardGame.BoardGame)}.{nameof(BoardGame.Category)}",
                $"{nameof(Pub.PubBoardGames)}.{nameof(PubBoardGame.BoardGame)}.{nameof(BoardGame.BoardGameRates)}",
                $"{nameof(Pub.Events)}.{nameof(Event.BoardGame)}",
                $"{nameof(Pub.Events)}.{nameof(Event.ParticipantEvents)}");
        }

        public override async Task<Maybe<Pub>> UpdateAsync(Pub updatedPub)
        {
            var existingPub = await _pubRepository.GetSingleOrMaybeAsync(
                p => p.Id == updatedPub.Id,
                p => p.Address);
            if (!existingPub.HasValue)
                return Maybe<Pub>.Nothing;
            updatedPub.Address.Id = existingPub.Value.Address.Id;
            return existingPub.Value.AccountId == updatedPub.AccountId
                ? await _pubRepository.UpdateAsync(updatedPub)
                : throw new UserNotAllowedToUpdatePubException(updatedPub.Id, updatedPub.AccountId);
        }

        public async Task<Maybe<Pub>> DeleteByIdAsync(string pubId, string userAccountId)
        {
            var pub = await _pubRepository.GetAsync(pubId);
            if (!pub.HasValue)
                return Maybe<Pub>.Nothing;

            return pub.Value.AccountId == userAccountId
                ? await _pubRepository.DeleteAsync(pub.Value)
                : throw new UserNotAllowedToDeletePubException(pubId, userAccountId);
        }

        public async Task<Maybe<Pub>> AddBoardGames(string pubId, string accountId, string boardGameId)
        {
            var existingPub = await _pubRepository.GetAsync(pubId);
            if (!existingPub.HasValue)
                return Maybe<Pub>.Nothing;

            if (existingPub.Value.AccountId != accountId)
                throw new UserNotAllowedToUpdatePubException(pubId, accountId);

            existingPub.Value.PubBoardGames.Add(new PubBoardGame
            {
                BoardGameId = boardGameId,
                PubId = pubId
            });

            return await _pubRepository.UpdateAsync(existingPub.Value);
        }

        public async Task<Maybe<Pub>> DeleteBoardGames(string pubId, string accountId, string boardGameId)
        {
            var existingPub = await _pubRepository.GetSingleOrMaybeAsync(
                pub => pub.Id == pubId,
                pub => pub.PubBoardGames);
            if (!existingPub.HasValue)
                return Maybe<Pub>.Nothing;

            if (existingPub.Value.AccountId != accountId)
                throw new UserNotAllowedToUpdatePubException(pubId, accountId);

            var boardGame = existingPub.Value.PubBoardGames.FirstOrDefault(game => game.BoardGameId == boardGameId);
            if (boardGame != null)
                await _pubBoardGameRepository.DeleteAsync(boardGame);

            return existingPub;
        }

        public async Task<Maybe<Pub>> LikePub(string boardGameId, HexadoUser user)
        {
            var pub = await _pubRepository.GetAsync(boardGameId);
            if (!pub.HasValue)
                return pub;
            pub.Value.LikedPubs.Add(
                new LikedPub
                {
                    HexadoUserId = user.Id,
                    HexadoUser = user
                });
            return await _pubRepository.UpdateAsync(pub.Value);
        }

        public Maybe<IEnumerable<string>> GetCities()
        {
            return _pubRepository.GetCities();
        }

        public async Task<Maybe<Pub>> SetImagePath(string id, string imagePath)
        {
            var pub = await _pubRepository.GetSingleOrMaybeAsync(
                p => p.Id == id,
                p => p.Address);
            if (!pub.HasValue)
                return pub;

            pub.Value.ImagePath = imagePath;
            return await _pubRepository.UpdateAsync(pub.Value);
        }
    }
}