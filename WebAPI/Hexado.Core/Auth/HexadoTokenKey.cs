using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Hexado.Core.Auth
{
    public static class HexadoTokenKey
    {
        public static SymmetricSecurityKey Get(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }
}