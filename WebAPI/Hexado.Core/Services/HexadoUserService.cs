using System.Threading.Tasks;
using Hexado.Core.Auth;
using Hexado.Db.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Core.Services
{
    public interface IHexadoUserService
    {
        Task<IdentityResult> CreateAsync(HexadoUser user, string password);
        Task<JwtTokenResult> Login(string email, string password);
    }

    public class HexadoUserService : IHexadoUserService
    {
        private readonly UserManager<HexadoUser> _userManager;
        private readonly IJwtTokenFactory _jwtTokenFactory;

        public HexadoUserService(
            UserManager<HexadoUser> userManager,
            IJwtTokenFactory jwtTokenFactory)
        {
            _userManager = userManager;
            _jwtTokenFactory = jwtTokenFactory;
        }

        public async Task<IdentityResult> CreateAsync(HexadoUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<JwtTokenResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? await _userManager.FindByNameAsync(email);
            if (!(user != null && await _userManager.CheckPasswordAsync(user, password)))
                return JwtTokenResult.Invalid();

            return _jwtTokenFactory.GenerateToken(user);
        }
    }
}