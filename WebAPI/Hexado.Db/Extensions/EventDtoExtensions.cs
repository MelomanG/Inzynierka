using System.Collections.Generic;
using System.Linq;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Db.Extensions
{
    public static class EventDtoExtensions
    {
        public static EventDto ToDto(this Event entity, bool isUserParticipant = false, bool isUserEvent = false)
        {
            return new EventDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                Address = new EventAddress
                {
                    City = entity.Address.City,
                    LocalNumber = entity.Address.LocalNumber,
                    BuildingNumber = entity.Address.BuildingNumber,
                    Street = entity.Address.Street,
                    PostalCode = entity.Address.PostalCode
                },
                ImagePath = entity.ImagePath,
                IsPublic = entity.IsPublic,
                Owner = entity.Owner,
                Pub = entity.Pub,
                BoardGame = entity.BoardGame,
                Participants = entity.ParticipantEvents,
                IsUserEvent = isUserEvent,
                IsUserParticipant = isUserParticipant
            };
        }

        public static PaginationResult<EventDto> ToDto(this PaginationResult<Event> entity)
        {
            return new PaginationResult<EventDto>
            {
                Page = entity.Page,
                PageCount = entity.PageCount,
                PageSize = entity.PageSize,
                Results = entity.Results.Select(p => p.ToDto()).ToList(),
                TotalCount = entity.TotalCount
            };
        }

        public static IEnumerable<EventDto> ToDto(this IEnumerable<Event> entities)
        {
            return entities.Select(bg => bg.ToDto());
        }
    }
}