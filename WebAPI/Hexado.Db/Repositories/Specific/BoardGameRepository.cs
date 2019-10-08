using Hexado.Db.Entities;

namespace Hexado.Db.Repositories.Specific
{
    public class BoardGameRepository : Repository<BoardGame>, IBoardGameRepository
    {
        public BoardGameRepository(
            HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }
    }
}