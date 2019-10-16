using Hexado.Core.Queries;
using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Core.Speczillas
{
    public interface IPubSpeczilla
    {
        ISpecification<Pub> GetSpecification(PubQuery query);
    }

    public class PubSpeczilla : IPubSpeczilla
    {
        public ISpecification<Pub> GetSpecification(PubQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}