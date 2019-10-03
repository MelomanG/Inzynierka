using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Auth;
using Hexado.Db;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services
{
    public class HexadoUserService : IHexadoUserService
    {
        private readonly IRepository<HexadoUser> _hexadoUserRepository;
        private readonly UserManager<HexadoUser> _userManager;
        private readonly ITokenFactory _tokenFactory;

        public HexadoUserService(
            IRepository<HexadoUser> hexadoUserRepository,
            UserManager<HexadoUser> userManager,
            ITokenFactory tokenFactory)
        {
            _hexadoUserRepository = hexadoUserRepository;
            _userManager = userManager;
            _tokenFactory = tokenFactory;
        }

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Maybe<Token>> LoginAsync(string loginName, string password)
        {
            var user = await _userManager.FindByEmailAsync(loginName) ?? await _userManager.FindByNameAsync(loginName);
            if (!(user != null && await _userManager.CheckPasswordAsync(user, password)))
                return Maybe<Token>.Nothing;

            return await GenerateTokenAsync(user);
        }

        public async Task<Maybe<Token>> RefreshTokenAsync(string email, string refreshToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.IsValidRefreshToken(refreshToken))
                return Maybe<Token>.Nothing;

            user.RemoveRefreshToken(refreshToken);
            return await GenerateTokenAsync(user);
        }

        private async Task<Maybe<Token>> GenerateTokenAsync(HexadoUser user)
        {
            var accessToken = _tokenFactory.GenerateAccessToken(user.Email);
            var refreshToken = _tokenFactory.GenerateRefreshToken(user.Id);
            if (!accessToken.HasValue || !refreshToken.HasValue)
                return Maybe<Token>.Nothing;

            user.RefreshTokens.Add(refreshToken.Value);
            await _hexadoUserRepository.UpdateAsync(user);

            return new Token(
                accessToken.Value,
                refreshToken.Value
            ).ToMaybe();
        }
    }
}