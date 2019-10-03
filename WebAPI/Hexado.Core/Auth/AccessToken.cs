using System;

namespace Hexado.Core.Auth
{
    public class AccessToken
    {
        //public string Token
        //{
        //    get => Token;
        //    private set
        //    {
        //        if (string.IsNullOrWhiteSpace(value))
        //            throw new ArgumentNullException(nameof(Token));
        //        Token = value;
        //    }
        //}

        public string Token { get; private set; }
        public int ExpiresIn { get; private set; }


        public AccessToken(string token, int expiresIn)
        {
            Token = token ?? throw new ArgumentNullException($"{nameof(AccessToken)}.{nameof(token)}");
            ExpiresIn = expiresIn;
        }
    }
}