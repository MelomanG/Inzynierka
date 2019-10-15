using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public class RateService : BaseService<Rate>, IRateService
    {
        private readonly IBoardGameRepository _boardGameRepository;

        public RateService(
            IRateRepository repository,
            IBoardGameRepository boardGameRepository)
            : base(repository)
        {
            _boardGameRepository = boardGameRepository;
        }

        public async Task<Maybe<BoardGame>> RateBoardGame(string id, Rate rate)
        {
            var result = await _boardGameRepository.GetAsync(id);
            if(!result.HasValue)
                return result;

            result.Value.Rates.Add(rate);
            return await _boardGameRepository.UpdateAsync(result.Value);
        }
    }
}