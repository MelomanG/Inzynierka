using System;

namespace Hexado.Core.Services.Exceptions
{
    public class UserNotAllowedToUpdateEventException : Exception
    {
        public UserNotAllowedToUpdateEventException(string eventId, string ownerId)
            : base($"OwnerId: {ownerId} is not allowed to update eventId {eventId}!")
        {
        }
    }
}