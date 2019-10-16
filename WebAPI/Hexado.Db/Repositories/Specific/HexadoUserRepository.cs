using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Constants;
using Hexado.Db.Entities;
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

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, HexadoRole.Admin));
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
    }
}