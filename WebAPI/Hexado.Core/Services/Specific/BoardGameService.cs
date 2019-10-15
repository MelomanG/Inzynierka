using System.Collections.Generic;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public class BoardGameService : BaseService<BoardGame>, IBoardGameService
    {
        public BoardGameService(IBoardGameRepository boardGameRepository)
            : base(boardGameRepository)
        {
        }
    }
}