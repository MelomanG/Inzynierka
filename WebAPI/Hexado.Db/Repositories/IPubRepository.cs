using Hexado.Db.Entities;

namespace Hexado.Db.Repositories
{
    public interface IPubRepository: IRepository<Pub>
    {
    }

    public class PubRepository : Repository<Pub>, IPubRepository
    {
        public PubRepository(
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }
    }
}