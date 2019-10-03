using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Auth;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services
{
    public interface IHexadoUserService
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
        Task<Maybe<Token>> Login(string email, string password);
    }

    public class HexadoUserService : IHexadoUserService
    {
        private readonly UserManager<HexadoUser> _userManager;
        private readonly ITokenFactory _tokenFactory;

        public HexadoUserService(
            UserManager<HexadoUser> userManager,
            ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _tokenFactory = tokenFactory;
        }

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Maybe<Token>> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? await _userManager.FindByNameAsync(email);
            if (!(user != null && await _userManager.CheckPasswordAsync(user, password)))
                return Maybe<Token>.Nothing;

            var accessToken = _tokenFactory.GenerateAccessToken(user);
            var refreshToken = _tokenFactory.GenerateRefreshToken(user.Id);
            if (!accessToken.HasValue || !refreshToken.HasValue)
                return Maybe<Token>.Nothing;

            //TODO: Save refresh token in DB

            return new Token(
                accessToken.Value,
                refreshToken.Value
                ).ToMaybe();
        }
    }
}