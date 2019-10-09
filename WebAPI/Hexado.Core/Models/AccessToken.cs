using System;

namespace Hexado.Core.Models
{
    public class AccessToken
    {
        public string Token { get; private set; }
        public int ExpiresIn { get; private set; }


        public AccessToken(string token, int expiresIn)
        {
            Token = token ?? throw new ArgumentNullException($"{nameof(AccessToken)}.{nameof(Token)}");
            ExpiresIn = expiresIn;
        }
    }
}