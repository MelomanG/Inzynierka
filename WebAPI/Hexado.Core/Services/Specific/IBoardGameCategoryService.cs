using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IBoardGameCategoryService: IBaseService<BoardGameCategory>
    {
    }

    public class BoardGameCategoryService : BaseService<BoardGameCategory>, IBoardGameCategoryService
    {
        public BoardGameCategoryService(
            IBoardGameCategoryRepository repository)
            : base(repository)
        {
        }
    }
}