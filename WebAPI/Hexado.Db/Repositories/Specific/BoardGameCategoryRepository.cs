using Hexado.Db.Entities;

namespace Hexado.Db.Repositories.Specific
{
    public interface IBoardGameCategoryRepository: IRepository<BoardGameCategory>
    {
    }

    public class BoardGameCategoryRepository : Repository<BoardGameCategory>, IBoardGameCategoryRepository
    {
        public BoardGameCategoryRepository(HexadoDbContext hexadoDbContext)
            : base(hexadoDbContext)
        {
        }
    }
}