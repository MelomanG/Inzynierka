using System.Linq;
using Hexado.Core.Queries;
using Hexado.Core.Speczillas.Specifications;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Speczilla.Constants;

namespace Hexado.Core.Speczillas
{
    public interface IEventSpeczilla
    {
        ISpecification<Event> GetSpecification(EventQuery query);
    }

    public class EventSpeczilla : IEventSpeczilla
    {
        public ISpecification<Event> GetSpecification(EventQuery query)
        {
            var specification = new EventSpecification();

            specification.SetPage(query.Page);
            specification.SetPageSize(query.PageSize);

            specification
                .AddInclude(bg => bg.BoardGame)
                .AddInclude(pub => pub.Address)
                .AddInclude(pub => pub.Pub)
                .AddInclude(pub => pub.ParticipantEvents)
                .AddInclude(pub => pub.Owner);

            if (!string.IsNullOrWhiteSpace(query.BoardGameId))
            {
                specification
                    .AndAlso(e => e.BoardGameId == query.BoardGameId);
            }

            if (!string.IsNullOrWhiteSpace(query.PubId))
            {
                specification
                    .AndAlso(e => e.PubId == query.PubId);
            }

            if (!string.IsNullOrWhiteSpace(query.City))
            {
                specification
                    .AndAlso(e => e.Address.City == query.City);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                specification
                    .AndAlso(e => e.Name.Contains(query.Search));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderBy))
                SetOrderBy(specification, query.OrderBy);

            return specification;
        }

        private static void SetOrderBy(Specification<Event> specification, string queryOrderBy)
        {
            var sortParam = queryOrderBy.Split(' ').ToList();
            if (sortParam.Count > 2)
                return;

            var isDescending = sortParam.Contains(QueryParamKey.Desc);
            switch (sortParam[0])
            {
                case "name":
                    specification.SetOrderBy(bg => bg.Name, isDescending);
                    break;

                case "users":
                    specification.SetOrderBy(p => p.ParticipantEvents.Count, isDescending);
                    break;

                case "date":
                    specification.SetOrderBy(p => p.StartDate, isDescending);
                    break;

                default:
                    specification.SetOrderBy(bg => bg.Name);
                    break;
            }
        }
    }
}