using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Repositories.Specific
{
    public interface IHexadoUserRepository: IRepository<HexadoUser>
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
        Task<Maybe<HexadoUser>> GetUserIncludeTokensAsync(Expression<Func<HexadoUser, bool>> expression);
        Maybe<IEnumerable<BoardGameDto>> GetLikedBoardGames(string userEmail);
        Maybe<IEnumerable<PubDto>> GetLikedPubs(string userEmail);
        Task<Maybe<IEnumerable<PubDto>>> GetUserPubsAsync(string userEmail);
        Task<Maybe<IEnumerable<EventDto>>> GetUserEventsAsync(string userEmail);
        Task<Maybe<IEnumerable<EventDto>>> GetUserParticipatedEvents(string userEmail);
        Task<Maybe<IEnumerable<EventDto>>> GetUserOwnedEvents(string userEmail);
    }
}