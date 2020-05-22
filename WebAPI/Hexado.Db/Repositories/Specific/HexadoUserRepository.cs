using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Db.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hexado.Db.Repositories.Specific
{
    public class HexadoUserRepository : Repository<HexadoUser>, IHexadoUserRepository
    {
        private readonly UserManager<HexadoUser> _userManager;

        public HexadoUserRepository(
            UserManager<HexadoUser> userManager,
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
            _userManager = userManager;
        }

        public override Task<Maybe<HexadoUser>> GetSingleOrMaybeAsync(Expression<Func<HexadoUser, bool>> predicate)
        {
            return base.GetSingleOrMaybeAsync(predicate, user => user.Account);
        }

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result;

            //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, HexadoRole.Admin));
            return await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
        }

        public async Task<Maybe<HexadoUser>> GetUserIncludeTokensAsync(Expression<Func<HexadoUser, bool>> expression)
        {
            return (await HexadoDbContext.HexadoUsers
                .Where(expression)
                .Include(user => user.RefreshTokens)
                .SingleOrDefaultAsync())
                .ToMaybe();
        }

        public Maybe<IEnumerable<BoardGameDto>> GetLikedBoardGames(string userEmail)
        {
            var likedBoardGames = HexadoDbContext.HexadoUsers
                .AsNoTracking()
                .Where(hu => hu.Email == userEmail)
                .Include(user => user.LikedBoardGames)
                    .ThenInclude(lbg => lbg.BoardGame)
                        .ThenInclude(bg => bg.Category)
                .SelectMany(u => u.LikedBoardGames)
                .Select(lbg => new
                {
                    lbg.BoardGame,
                    AmountOfLikes = lbg.BoardGame.LikedBoardGames.Count
                })
                .ToList()
                .Select(x => x.BoardGame.ToDto(x.AmountOfLikes))
                .ToList()
                .ToMaybe();

            return likedBoardGames.HasValue
                ? likedBoardGames.Value.AsEnumerable().ToMaybe()
                : Maybe<IEnumerable<BoardGameDto>>.Nothing;
        }

        public Maybe<IEnumerable<PubDto>> GetLikedPubs(string userEmail)
        {
            var likedPubs = HexadoDbContext.HexadoUsers
                .AsNoTracking()
                .Where(hu => hu.Email == userEmail)
                .Include(user => user.LikedPubs)
                    .ThenInclude(lp => lp.Pub)
                        .ThenInclude(p => p.Address)
                .SelectMany(u => u.LikedPubs)
                .Select(lp => new
                {
                    lp.Pub,
                    AmountOfLikes = lp.Pub.LikedPubs.Count
                })
                .ToList()
                .Select(x => x.Pub.ToDto(x.AmountOfLikes))
                .ToList()
                .ToMaybe();

            return likedPubs.HasValue
                ? likedPubs.Value.AsEnumerable().ToMaybe()
                : Maybe<IEnumerable<PubDto>>.Nothing;
        }
    }
}