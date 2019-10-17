using System;

namespace Hexado.Core.Services.Exceptions
{
    public class UserNotAllowedToDeletePubException : Exception
    {
        public UserNotAllowedToDeletePubException(string pubId, string accountId)
            : base($"AccountId: {accountId} is not allowed to delete pubId {pubId}!")
        {
        }
    }
}