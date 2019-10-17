using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Entities
{
    public class HexadoUser : IdentityUser, IBaseEntity
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public UserAccount Account { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public ICollection<BoardGameRate> BoardGameRates { get; set; } = new HashSet<BoardGameRate>();
        public ICollection<PubRate> PubRates { get; set; } = new HashSet<PubRate>();


        public void RemoveRefreshToken(string refreshToken)
        {
            var token = RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (token != null)
                RefreshTokens.Remove(token);
        }
        
        public bool IsValidRefreshToken(string refreshToken)
        {
            //TODO: needs refactor
            var token = RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

            if (token == null)
                return false;

            if (token.IsActive) return true;
            RefreshTokens.Remove(token);
            return false;
        }
    }
}