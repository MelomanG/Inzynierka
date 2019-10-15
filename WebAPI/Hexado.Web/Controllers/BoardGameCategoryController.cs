using System;
using System.Threading.Tasks;
using Hexado.Core.Services.Specific;
using Hexado.Db.Constants;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class BoardGameCategoryController : ApiBaseController
    {
        private readonly IBoardGameCategoryService _boardGameCategoryService;
        private readonly ILogger _logger;

        public BoardGameCategoryController(
            IBoardGameCategoryService boardGameCategoryService,
            ILoggerFactory loggerFactory)
        {
            _boardGameCategoryService = boardGameCategoryService;
            _logger = loggerFactory.CreateLogger<BoardGameCategoryController>();
        }

        [HttpPost]
        [Authorize(Policy = HexadoPolicy.AdministratorOnly)]
        public async Task<IActionResult> Create(BoardGameCategoryModel model)
        {
            try
            {
                var result = await _boardGameCategoryService.CreateAsync(model.ToEntity());

                return result.HasValue 
                    ? CreatedJson(result.Value)
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new board game category!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _boardGameCategoryService.GetAllAsync();

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving board game categories!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = HexadoPolicy.AdministratorOnly)]
        public async Task<IActionResult> Update(string id, BoardGameCategoryModel model)
        {
            try
            {
                var result = await _boardGameCategoryService.UpdateAsync(model.ToEntity(id));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating board game category! " +
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
                var result = await _boardGameCategoryService.DeleteByIdAsync(id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting board game category! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }
    }
}