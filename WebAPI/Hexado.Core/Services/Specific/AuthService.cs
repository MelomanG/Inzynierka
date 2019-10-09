using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Auth;
using Hexado.Core.Models;
using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Hexado.Core.Services.Specific
{
    //==================================================================================
    //TODO: Authentication need to be refactored. Currently it's hard to test this class
    //==================================================================================
    public class AuthService : IAuthService
    {
        private readonly IHexadoUserRepository _hexadoUserRepository;
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly UserManager<HexadoUser> _userManager;
        private readonly JwtOptions _options;
        private readonly ITokenFactory _tokenFactory;

        public AuthService(
            IHexadoUserRepository hexadoUserRepository,
            IAccessTokenValidator accessTokenValidator,
            UserManager<HexadoUser> userManager,
            IOptions<JwtOptions> options,
            ITokenFactory tokenFactory)
        {
            _hexadoUserRepository = hexadoUserRepository;
            _accessTokenValidator = accessTokenValidator;
            _userManager = userManager;
            _options = options.Value;
            _tokenFactory = tokenFactory;
        }

        public async Task<Maybe<Token>> LoginAsync(string loginName, string password)
        {

            var user = await _userManager.FindByEmailAsync(loginName) ?? await _userManager.FindByNameAsync(loginName);
            if (!(user != null && await _userManager.CheckPasswordAsync(user, password)))
                return Maybe<Token>.Nothing;

            var claims = await _userManager.GetClaimsAsync(user);
            var token = _tokenFactory.GenerateToken(user.Id, claims);

            user.RefreshTokens.Add(token.Value.RefreshToken);
            await _hexadoUserRepository.UpdateAsync(user);

            return token;
        }

        public async Task<Maybe<Token>> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var claims =_accessTokenValidator.Validate(accessToken,
                HexadoTokenSpecific.GetValidationParameters(_options.Secret, false));
            if(!claims.HasValue || claims.Value.All(claim => claim.Type != ClaimTypes.Email))
                return Maybe<Token>.Nothing;

            var email = claims.Value
                .Single(c => c.Type == ClaimTypes.Email).Value;

            var user = await _hexadoUserRepository
                .GetUserIncludeTokensAsync(u => u.Email == email);

            if (!user.HasValue || !user.Value.IsValidRefreshToken(refreshToken))
                return Maybe<Token>.Nothing;

            var token = _tokenFactory.GenerateToken(user.Value.Id, claims.Value);
            if (!token.HasValue)
                return Maybe<Token>.Nothing;

            user.Value.RemoveRefreshToken(refreshToken);
            user.Value.RefreshTokens.Add(token.Value.RefreshToken);
            await _hexadoUserRepository.UpdateAsync(user.Value);

            return token;
        }
    }
}