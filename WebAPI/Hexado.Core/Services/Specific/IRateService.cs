using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;

namespace Hexado.Core.Services.Specific
{
    public interface IRateService: IBaseService<Rate>
    {
        Task<Maybe<BoardGame>> RateBoardGame(string id, Rate rate);
    }
}