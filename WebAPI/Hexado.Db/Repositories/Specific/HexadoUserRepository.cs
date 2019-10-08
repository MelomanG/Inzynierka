using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Functional.Maybe;
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

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
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