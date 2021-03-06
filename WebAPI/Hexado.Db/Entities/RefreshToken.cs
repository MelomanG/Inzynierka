﻿using System;

namespace Hexado.Db.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public string UserId { get; private set; }
        public virtual HexadoUser HexadoUser { get; private set; }
        public bool IsActive => ExpirationDate >= DateTime.UtcNow;

        public RefreshToken(string userId, string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
            UserId = userId;
        }
    }
}