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

        public virtual UserAccount Account { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
        public virtual ICollection<BoardGameRate> BoardGameRates { get; set; } = new HashSet<BoardGameRate>();
        public virtual ICollection<PubRate> PubRates { get; set; } = new HashSet<PubRate>();
        public virtual ICollection<LikedBoardGame> LikedBoardGames { get; set; } = new HashSet<LikedBoardGame>();
        public virtual ICollection<LikedPub> LikedPubs { get; set; } = new HashSet<LikedPub>();

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