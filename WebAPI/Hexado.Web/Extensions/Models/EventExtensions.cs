using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hexado.Db.Dtos;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Web.Models;
using Hexado.Web.Models.Responses;

namespace Hexado.Web.Extensions.Models
{
    public static class EventExtensions
    {
        public static Event ToEntity(this EventModel entity, string ownerId)
        {
            return new Event
            {
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                Address = entity.Address.ToEventAddressEntity(),
                IsPublic = entity.IsPublic,
                OwnerId = ownerId,
                PubId = entity.PubId,
                BoardGameId = entity.BoardGameId,
                ImagePath = entity.ImagePath
            };
        }

        public static EventResponse ToResponse(this Event entity, bool isUserEvent = false, bool isUserParticipant = false)
        {
            return new EventResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                Address = entity.Address?.ToResponse(),
                ImagePath = entity.ImagePath,
                IsPublic = entity.IsPublic,
                Owner = entity.Owner?.ToResponse(),
                Pub = entity.Pub?.ToResponse(),
                BoardGame = entity.BoardGame?.ToResponse(),
                Participants = entity.ParticipantEvents?.Select(pe => pe?.ToResponse()),
                IsUserEvent = isUserEvent,
                IsUserParticipant = isUserParticipant
            };
        }

        public static EventResponse ToResponse(this EventDto dto, bool isUserEvent = false, bool isUserParticipant = false)
        {
            return new EventResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                Address = dto.Address?.ToResponse(),
                ImagePath = dto.ImagePath,
                IsPublic = dto.IsPublic,
                Owner = dto.Owner?.ToResponse(),
                Pub = dto.Pub?.ToResponse(),
                BoardGame = dto.BoardGame?.ToResponse(),
                Participants = dto.Participants?.Select(p => p.ToResponse()),
                IsUserEvent = isUserEvent,
                IsUserParticipant = isUserParticipant
            };
        }

        public static IEnumerable<EventResponse> ToResponse(this IEnumerable<EventDto> dtos)
        {
            return dtos.Select(dto => dto.ToResponse(dto.IsUserEvent, dto.IsUserParticipant));
        }

        public static PaginationResult<EventResponse> ToResponse(this PaginationResult<Event> entity)
        {
            return new PaginationResult<EventResponse>
            {
                Page = entity.Page,
                PageCount = entity.PageCount,
                PageSize = entity.PageSize,
                Results = entity.Results.Select(p => p.ToResponse()).ToList(),
                TotalCount = entity.TotalCount
            };
        }

        public static HexadoUserResponse ToResponse(this ParticipantEvent entity)
        {
            return new HexadoUserResponse
            {
                Id = entity.ParticipantId,
                UserName = entity.Participant?.UserName
            };
        }
    }
}