using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Core.Speczillas.Specifications
{
    public interface IEventSpecification : ISpecification<Event>
    {
    }

    public sealed class EventSpecification : Specification<Event>, IEventSpecification
    {
        public EventSpecification()
            : base(bg => bg.Name, false)
        {
            AddInclude($"{nameof(Event.Address)}");
            AddInclude($"{nameof(Event.Pub)}");
            AddInclude($"{nameof(Event.BoardGame)}");
            AddInclude($"{nameof(Event.Owner)}");
            AddInclude($"{nameof(Event.ParticipantEvents)}.{nameof(ParticipantEvent.Participant)}");
            AndAlso(e => e.IsPublic);
        }
    }
}