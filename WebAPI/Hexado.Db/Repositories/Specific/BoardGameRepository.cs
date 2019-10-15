using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Microsoft.EntityFrameworkCore;

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

        public override async Task<Maybe<IEnumerable<BoardGame>>> GetAllAsync()
        {
            return (await HexadoDbContext.BoardGames.AsNoTracking()
                .Include(bg => bg.Category)
                .ToListAsync()).AsEnumerable().ToMaybe();
        }
    }
}