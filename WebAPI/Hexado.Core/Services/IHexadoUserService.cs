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
        Task<Maybe<Token>> LoginAsync(string loginName, string password);
        Task<Maybe<Token>> RefreshTokenAsync(string email, string refreshToken);
    }
}