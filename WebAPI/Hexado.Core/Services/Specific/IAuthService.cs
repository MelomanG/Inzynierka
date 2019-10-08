using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Auth;

namespace Hexado.Core.Services.Specific
{
    public interface IAuthService
    {
        Task<Maybe<Token>> LoginAsync(string loginName, string password);
        Task<Maybe<Token>> RefreshTokenAsync(string accessToken, string refreshToken);
    }
}