using Hexado.Db.Entities;

namespace Hexado.Db.Repositories.Specific
{
    public interface IContactRepository : IRepository<Contact>
    {
    }

    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }
    }
}