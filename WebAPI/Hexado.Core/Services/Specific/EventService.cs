using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Services.Exceptions;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IEventService : IBaseService<Event>
    {
        Task<Maybe<Event>> UpdateAsync(string id, Event updatedEvent);
        Task<Maybe<Event>> AddParticipantAsync(string eventId, string participantId);
        Task<Maybe<Event>> DeleteParticipantAsync(string eventId, string participantId);
        Task<Maybe<Event>> SetImagePath(string id, string imagePath);
        Maybe<IEnumerable<string>> GetCities();
    }

    public class EventService : BaseService<Event>, IEventService
    {
        private readonly IPubRepository _pubRepository;
        private readonly IRepository<ParticipantEvent> _participantEventRepository;
        private readonly IEventRepository _eventRepository;

        public EventService(
            IPubRepository pubRepository,
            IRepository<ParticipantEvent> participantEventRepository,
            IEventRepository eventRepository)
            : base(eventRepository)
        {
            _pubRepository = pubRepository;
            _participantEventRepository = participantEventRepository;
            _eventRepository = eventRepository;
        }

        public override async Task<Maybe<Event>> CreateAsync(Event entity)
        {
            if(entity.Address?.City != null)
                return await _eventRepository.CreateAsync(entity);
            
            var pub = await _pubRepository.GetSingleOrMaybeAsync(
                p => p.Id == entity.PubId,
                p => p.Address);
            if (!pub.HasValue)
                return Maybe<Event>.Nothing;

            entity.Address = new EventAddress
            {
                City = pub.Value.Address.City,
                LocalNumber = pub.Value.Address.LocalNumber,
                BuildingNumber = pub.Value.Address.BuildingNumber,
                Street = pub.Value.Address.Street,
                PostalCode = pub.Value.Address.PostalCode
            };

            return await _eventRepository.CreateAsync(entity);
        }


        public override Task<Maybe<Event>> GetByIdAsync(string id)
        {
            return _eventRepository.GetSingleOrMaybeAsync(e => e.Id == id,
                $"{nameof(Event.Address)}",
                $"{nameof(Event.Pub)}",
                $"{nameof(Event.Pub)}.{nameof(Pub.PubRates)}",
                $"{nameof(Event.Pub)}.{nameof(Pub.LikedPubs)}",
                $"{nameof(Event.Pub)}.{nameof(Pub.Address)}",
                $"{nameof(Event.BoardGame)}.{nameof(BoardGame.BoardGameRates)}",
                $"{nameof(Event.BoardGame)}.{nameof(BoardGame.LikedBoardGames)}",
                $"{nameof(Event.BoardGame)}",
                $"{nameof(Event.Owner)}",
                $"{nameof(Event.ParticipantEvents)}.{nameof(ParticipantEvent.Participant)}");
        }

        public async Task<Maybe<Event>> UpdateAsync(string id, Event updatedEvent)
        {
            var existingEvent = await _eventRepository.GetSingleOrMaybeAsync(
                e => e.Id == id,
                e => e.Address);

            if (!existingEvent.HasValue)
                return Maybe<Event>.Nothing;

            updatedEvent.Id = existingEvent.Value.Id;

            return existingEvent.Value.OwnerId == updatedEvent.OwnerId
                ? await _eventRepository.UpdateAsync(updatedEvent)
                : throw new UserNotAllowedToUpdateEventException(updatedEvent.Id, updatedEvent.OwnerId);
        }

        public async Task<Maybe<Event>> AddParticipantAsync(string eventId, string participantId)
        {
            var existingEvent = await _eventRepository.GetAsync(eventId);
            if (!existingEvent.HasValue)
                return Maybe<Event>.Nothing;

            existingEvent.Value.ParticipantEvents.Add(new ParticipantEvent
            {
                EventId = eventId,
                ParticipantId = participantId
            });

            return await _eventRepository.UpdateAsync(existingEvent.Value);
        }

        public async Task<Maybe<Event>> DeleteParticipantAsync(string eventId, string participantId)
        {
            var existingEvent = await _eventRepository.GetSingleOrMaybeAsync(
                e => e.Id == eventId,
                e => e.ParticipantEvents);
            if (!existingEvent.HasValue)
                return Maybe<Event>.Nothing;

            var participant = existingEvent.Value.ParticipantEvents.FirstOrDefault(p => p.ParticipantId == participantId);
            if (participant != null)
                await _participantEventRepository.DeleteAsync(participant);

            return existingEvent;
        }

        public async Task<Maybe<Event>> SetImagePath(string id, string imagePath)
        {
            var existingEvent = await _eventRepository.GetSingleOrMaybeAsync(
                p => p.Id == id);
            if (!existingEvent.HasValue)
                return existingEvent;

            existingEvent.Value.ImagePath = imagePath;
            return await _eventRepository.UpdateAsync(existingEvent.Value);
        }

        public Maybe<IEnumerable<string>> GetCities()
        {
            return _eventRepository.GetCities();
        }

        public override async Task<Maybe<Event>> DeleteByIdAsync(string id)
        {
            var existingEvent = await _eventRepository.GetSingleOrMaybeAsync(e => e.Id == id,
                $"{nameof(Event.ParticipantEvents)}");

            if (!existingEvent.HasValue)
                return existingEvent;

            foreach (var participantEvent in existingEvent.Value.ParticipantEvents)
            {
                await _participantEventRepository.DeleteByIdAsync(participantEvent.Id);
            }

            return await _eventRepository.DeleteByIdAsync(id);
        }
    }
}