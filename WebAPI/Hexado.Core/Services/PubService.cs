using System.Threading.Tasks;
using System.Linq;
using Functional.Maybe;
using Hexado.Core.Services.Exceptions;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;

namespace Hexado.Core.Services
{
    public interface IPubService : IBaseService<Pub>
    {
        Task<Maybe<Pub>> DeleteByIdAsync(string pubId, string userAccountId);
        Task<Maybe<Pub>> AddBoardGames(string pubId, string accountId, string[] boardGameIds);
        Task<Maybe<Pub>> DeleteBoardGames(string pubId, string accountId, string[] boardGameIds);
    }

    public class PubService : BaseService<Pub>, IPubService
    {
        public PubService(
            IRepository<Pub> repository)
            : base(repository)
        {
        }

        public override async Task<Maybe<Pub>> UpdateAsync(Pub updatedPub)
        {
            var existingPub = await Repository.GetAsync(updatedPub.Id);
            if (!existingPub.HasValue)
                return Maybe<Pub>.Nothing;

            return existingPub.Value.AccountId == updatedPub.AccountId
                ? await Repository.UpdateAsync(updatedPub)
                : throw new UserNotAllowedToUpdatePubException(updatedPub.Id, updatedPub.AccountId);
        }

        public async Task<Maybe<Pub>> DeleteByIdAsync(string pubId, string userAccountId)
        {
            var pub = await Repository.GetAsync(pubId);
            if (!pub.HasValue)
                return Maybe<Pub>.Nothing;

            return pub.Value.AccountId == userAccountId
                ? await Repository.DeleteAsync(pub.Value)
                : throw new UserNotAllowedToDeletePubException(pubId, userAccountId);
        }

        public async Task<Maybe<Pub>> AddBoardGames(string pubId, string accountId, string[] boardGameIds)
        {
            var existingPub = await Repository.GetAsync(pubId);
            if (!existingPub.HasValue)
                return Maybe<Pub>.Nothing;

            if (existingPub.Value.AccountId != accountId)
                throw new UserNotAllowedToUpdatePubException(pubId, accountId);

            foreach (var boardGameId in boardGameIds)
            {
                existingPub.Value.PubBoardGames.Add(new PubBoardGame
                {
                    BoardGameId = boardGameId,
                    PubId = pubId
                });
            }

            return await Repository.UpdateAsync(existingPub.Value);
        }

        public async Task<Maybe<Pub>> DeleteBoardGames(string pubId, string accountId, string[] boardGameIds)
        {
            var existingPub = await Repository.GetSingleOrMaybeAsync(
                pub => pub.Id == pubId,
                pub => pub.PubBoardGames);
            if (!existingPub.HasValue)
                return Maybe<Pub>.Nothing;

            if (existingPub.Value.AccountId != accountId)
                throw new UserNotAllowedToUpdatePubException(pubId, accountId);

            foreach (var boardGameId in boardGameIds)
            {
                var boardGame = existingPub.Value.PubBoardGames.FirstOrDefault(game => game.BoardGameId == boardGameId);
                if (boardGame != null)
                    existingPub.Value.PubBoardGames.Remove(boardGame);
            }

            return await Repository.UpdateAsync(existingPub.Value);
        }
    }
}