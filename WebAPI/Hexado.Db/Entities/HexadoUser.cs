using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Hexado.Db.Entities
{
    public class HexadoUser : IdentityUser, IBaseEntity
    {
        public string FullName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public HexadoUser()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }


        public void RemoveRefreshToken(string refreshToken)
        {
            var token = RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (token != null)
                RefreshTokens.Remove(token);
        }


        public bool IsValidRefreshToken(string refreshToken)
        {
            //TODO: needs refactor
            var token =  RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

            if (token == null)
                return false;

            if (token.IsActive) return true;
            RefreshTokens.Remove(token);
            return false;

        }
    }
}