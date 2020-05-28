using Hexado.Db.Entities;
using Hexado.Db.Repositories;

namespace Hexado.Core.Services.Specific
{
    public interface IBoardGameCategoryService : IBaseService<BoardGameCategory>
    {
    }

    public class BoardGameCategoryService : BaseService<BoardGameCategory>, IBoardGameCategoryService
    {
        public BoardGameCategoryService(
            IRepository<BoardGameCategory> contactRepository)
            : base(contactRepository)
        {
        }
    }
}