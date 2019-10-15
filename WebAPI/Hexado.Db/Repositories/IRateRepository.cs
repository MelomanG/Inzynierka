using Hexado.Db.Entities;

namespace Hexado.Db.Repositories
{
    public interface IRateRepository : IRepository<Rate>
    {

    }

    public class RateRepository : Repository<Rate>, IRateRepository
    {
        public RateRepository(HexadoDbContext hexadoDbContext) 
            : base(hexadoDbContext)
        {
        }
    }
}