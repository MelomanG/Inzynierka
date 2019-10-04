using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Functional.Maybe;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Repositories
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