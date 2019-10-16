using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Db.Repositories.Specific
{
    public interface IBoardGameRepository: IRepository<BoardGame>
    {
    }

    public class BoardGameRepository : Repository<BoardGame>, IBoardGameRepository
    {
        public BoardGameRepository(
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }

        public override Task<Maybe<IEnumerable<BoardGame>>> GetAllAsync()
        {
            return base.GetAllAsync(bg => bg.Category);
        }
    }
}