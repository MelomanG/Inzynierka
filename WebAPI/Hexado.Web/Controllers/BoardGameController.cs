using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Queries;
using Hexado.Core.Services.Specific;
using Hexado.Core.Speczillas;
using Hexado.Db.Constants;
using Hexado.Db.Entities;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class BoardGameController : ApiBaseController
    {
        private readonly IBoardGameService _boardGameService;
        private readonly IRateService _rateService;
        private readonly IBoardGameSpeczilla _boardGameSpeczilla;
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger _logger;

        public BoardGameController(
            IBoardGameService boardGameService,
            IRateService rateService,
            IHexadoUserService hexadoUserService,
            IBoardGameSpeczilla boardGameSpeczilla,
            ILoggerFactory loggerFactory)
        {
            _boardGameService = boardGameService;
            _boardGameSpeczilla = boardGameSpeczilla;
            _hexadoUserService = hexadoUserService;
            _rateService = rateService;
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
                    ? CreatedJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new board game!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BoardGameQuery query)
        {
            try
            {
                var specification = _boardGameSpeczilla.GetSpecification(query);
                var result = await _boardGameService.GetPaginationResultAsync(specification);
                if (!result.HasValue)
                    return NotFound();

                var responseResult = result.Value.ToResponse();

                if (!string.IsNullOrWhiteSpace(UserEmail))
                {
                    var likedBoardGames = _hexadoUserService.GetLikedBoardGames(UserEmail);
                    if (likedBoardGames.HasValue)
                    {
                        foreach (var likedBoardGame in likedBoardGames.Value)
                        {
                            var boardGame = responseResult.Results
                                .SingleMaybe(rr => rr.Id == likedBoardGame.Id);
                            if (boardGame.HasValue)
                                boardGame.Value.IsLikedByUser = true;
                        }
                    }
                }

                return OkJson(responseResult);
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
                if (!result.HasValue)
                    return NotFound();

                var responseResult = result.Value.ToResponse();
                if (!string.IsNullOrWhiteSpace(UserEmail))
                {
                    var likedBoardGames = _hexadoUserService.GetLikedBoardGames(UserEmail);
                    if (likedBoardGames.HasValue)
                        if(likedBoardGames.Value.Any(lbg => lbg.Id == responseResult.Id))
                                responseResult.IsLikedByUser = true;
                }

                return OkJson(responseResult);
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

                var notFullPath = Path.Combine("Images", "BoardGames");
                var fromRootPath = Path.Combine(Directory.GetCurrentDirectory(), notFullPath);
                ValidateIfStaticFileExists(id, fromRootPath);
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

        [HttpPost("{id}/Rate")]
        [Authorize]
        public async Task<IActionResult> RateBoardGame(string id, RateModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _rateService.RateBoardGame(
                    model.ToBoardGameRateEntity(
                        user.Value.Id,
                        id));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while applying rate to board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("{id}/Rate")]
        [Authorize]
        public async Task<IActionResult> GetRateBoardGame(string id)
        {
            try
            {
                var rate = await _hexadoUserService.GetUserBoardGameRate(id, UserEmail);

                return rate.HasValue
                    ? OkJson(rate.Value.ToRateResponse())
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while applying rate to board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPut("{id}/Rate/{rateId}")]
        [Authorize]
        public async Task<IActionResult> UpdateBoardGameRate(string id, string rateId, RateModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _rateService.UpdateBoardGameRate(
                    model.ToBoardGameRateEntity(
                        user.Value.Id,
                        id,
                        rateId));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating board game rate! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("Rate/{rateId}")]
        [Authorize]
        public async Task<IActionResult> DeleteBoardGameRate(string rateId)
        {
            try
            {
                var result = await _rateService.DeleteBoardGameRate(rateId);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting board game rate! " +
                                     $"Id: {rateId}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/like")]
        [Authorize]
        public async Task<IActionResult> LikeBoardGame(string id)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _boardGameService.LikeBoardGame(id, user.Value);

                if (result.HasValue)
                    return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while liking board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("like")]
        [Authorize]
        public  IActionResult GetLikedBoardGames()
        {
            try
            {
                var likedBoardGames = _hexadoUserService.GetLikedBoardGames(UserEmail);

                return likedBoardGames.HasValue
                    ? OkJson(likedBoardGames.Value.ToResponse(true))
                    : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching liked board game! ");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}/unlike")]
        [Authorize]
        public async Task<IActionResult> UnLikeBoardGame(string id)
        {
            try
            {
                await _hexadoUserService.UnLikeBoardGameAsync(UserEmail, id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while liking board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }


        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadFile(string id, IFormFile image)
        {
            try
            {
                var notFullPath = Path.Combine("Images", "BoardGames");
                var fromRootPath = Path.Combine(Directory.GetCurrentDirectory(), notFullPath);
                ValidateIfStaticFileExists(id, fromRootPath);

                var imageName = id + image.FileName;
                var fullRootPath = Path.Combine(fromRootPath, imageName);
                var staticFilePath = Path.Combine(notFullPath, imageName);
                var result = Maybe<BoardGame>.Nothing;
                await using (var stream = new FileStream(fullRootPath, FileMode.Create))
                {
                    image.CopyTo(stream);
                    result = await _boardGameService.SetImagePath(id, staticFilePath);
                }

                if (!result.HasValue)
                    return NotFound();
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading image for board game! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        //[HttpDelete("clear")]
        //public async Task<IActionResult> Clear()
        //{
        //    await _boardGameService.ClearAsync();
        //    return Ok();
        //}
    }
}