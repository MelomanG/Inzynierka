using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Entities
{
    public class HexadoUser : IdentityUser
    {
        public string FullName { get; set; }

        public HexadoUser()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Add(refreshToken);
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            var token = RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (token != null)
                RefreshTokens.Remove(token);
        }

        public bool IsValidRefreshToken(string refreshToken) => 
            RefreshTokens.Any(token => 
                token.Token == refreshToken &&
                token.IsActive);
    }
}