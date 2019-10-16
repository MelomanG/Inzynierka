using Hexado.Db.Entities;
using Hexado.Db.Repositories;

namespace Hexado.Core.Services
{
    public interface IPubService: IBaseService<Pub>
    {
    }

    public class PubService : BaseService<Pub>, IPubService
    {
        public PubService(
            IRepository<Pub> repository)
            : base(repository)
        {
        }
    }
}