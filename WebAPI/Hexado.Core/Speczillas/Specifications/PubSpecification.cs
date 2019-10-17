using Hexado.Db.Entities;
using Hexado.Speczilla;

namespace Hexado.Core.Speczillas.Specifications
{
    public interface IPubSpecification : ISpecification<Pub>
    {
    }

    public sealed class PubSpecification : Specification<Pub>, IPubSpecification
    {
        public PubSpecification()
            : base(bg => bg.Name, false)
        {
        }
    }
}