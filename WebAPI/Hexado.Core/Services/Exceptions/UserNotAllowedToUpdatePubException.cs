using System;

namespace Hexado.Core.Services.Exceptions
{
    public class UserNotAllowedToUpdatePubException : Exception
    {
        public UserNotAllowedToUpdatePubException(string pubId, string accountId)
            : base($"AccountId: {accountId} is not allowed to update pubId {pubId}!")
        {
        }
    }
}