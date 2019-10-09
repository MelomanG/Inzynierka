using System;
using System.Threading.Tasks;
using Hexado.Core.Services.Specific;
using Hexado.Db.Constants;
using Hexado.Db.Entities;
using Hexado.Speczilla;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class BoardGameController : ApiBaseController
    {
        private readonly IBoardGameService _boardGameService;
        private readonly ISpecificationFactory<BoardGame> _specificationFactory;
        private readonly ILogger<BoardGameController> _logger;

        public BoardGameController(
            IBoardGameService boardGameService,
            ISpecificationFactory<BoardGame> specificationFactory,
            ILoggerFactory loggerFactory)
        {
            _boardGameService = boardGameService;
            _specificationFactory = specificationFactory;
            _logger = loggerFactory.CreateLogger<BoardGameController>();
        }

        [HttpPost]
        [Authorize(Policy = HexadoPolicy.AdministratorOnly)]
        public async Task<IActionResult> Create(BoardGameModel model)
        {
            try
            {
                var result = await _boardGameService.CreateAsync(model.ToEntity());

                return result.HasValue 
                    ? CreatedJson(result.Value)
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new board game!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var specification = _specificationFactory.CreateSpecification(HttpContext.Request.Query);
                var result = await _boardGameService.GetPaginationResultAsync(specification);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving board games!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _boardGameService.GetByIdAsync(id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = HexadoPolicy.AdministratorOnly)]
        public async Task<IActionResult> Update(string id, BoardGameModel model)
        {
            try
            {
                var result = await _boardGameService.UpdateAsync(model.ToEntity(id));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = HexadoPolicy.AdministratorOnly)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _boardGameService.DeleteByIdAsync(id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }
    }
}