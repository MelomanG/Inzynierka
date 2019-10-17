using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Services.Exceptions;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;

namespace Hexado.Core.Services
{
    public interface IPubService : IBaseService<Pub>
    {
        Task<Maybe<Pub>> DeleteByIdAsync(string pubId, string userAccountId);
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
            if(!pub.HasValue)
                return Maybe<Pub>.Nothing;

            return pub.Value.AccountId == userAccountId
                ? await Repository.DeleteAsync(pub.Value)
                : throw new UserNotAllowedToDeletePubException(pubId, userAccountId);
        }
    }
}