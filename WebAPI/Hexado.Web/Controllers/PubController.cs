using System;
using System.Threading.Tasks;
using Hexado.Core.Queries;
using Hexado.Core.Services;
using Hexado.Core.Services.Exceptions;
using Hexado.Core.Services.Specific;
using Hexado.Core.Speczillas;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class PubController : ApiBaseController
    {
        private readonly IPubService _pubService;
        private readonly IPubSpeczilla _pubSpeczilla;
        private readonly IBoardGameService _boardGameService;
        private readonly IBoardGameSpeczilla _boardGameSpeczilla;
        private readonly IRateService _rateService;
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger _logger;

        public PubController(
            IPubService pubService,
            IPubSpeczilla pubSpeczilla,
            IBoardGameService boardGameService,
            IBoardGameSpeczilla boardGameSpeczilla,
            IRateService rateService,
            IHexadoUserService hexadoUserService,
            ILoggerFactory loggerFactory)
        {
            _pubService = pubService;
            _pubSpeczilla = pubSpeczilla;
            _boardGameService = boardGameService;
            _boardGameSpeczilla = boardGameSpeczilla;
            _rateService = rateService;
            _hexadoUserService = hexadoUserService;
            _logger = loggerFactory.CreateLogger<PubController>();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PubModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _pubService.CreateAsync(model.ToEntity(user.Value.Account.Id));

                return result.HasValue 
                    ? CreatedJson(result.Value)
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new pub!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PubQuery query)
        {
            try
            {
                var specification = _pubSpeczilla.GetSpecification(query);
                var result = await _pubService.GetPaginationResultAsync(specification);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving pubs!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _pubService.GetByIdAsync(id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving pub! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, PubModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _pubService.UpdateAsync(
                    model.ToEntity(
                        user.Value.Account.Id,
                        id));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (UserNotAllowedToUpdatePubException ex)
            {
                _logger.LogWarning(ex, $"User: {UserEmail} not allowed to update pubId: {id}!");
                return ForbiddenJson();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating pub! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _pubService.DeleteByIdAsync(id, user.Value.Account.Id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (UserNotAllowedToDeletePubException ex)
            {
                _logger.LogWarning(ex, $"User: {UserEmail} not allowed to delete pubId: {id}!");
                return ForbiddenJson();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting pub! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/Rate")]
        [Authorize]
        public async Task<IActionResult> RatePub(string id, RateModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _rateService.RatePub(
                    model.ToPubRateEntity(
                        user.Value.Id,
                        id));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while applying rate to pub! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPut("{id}/Rate/{rateId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePubRate(string id, string rateId, RateModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _rateService.UpdatePubRate(
                    model.ToPubRateEntity(
                        user.Value.Id,
                        id,
                        rateId));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating pub rate! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("Rate/{rateId}")]
        [Authorize]
        public async Task<IActionResult> DeletePubRate(string rateId)
        {
            try
            {
                var result = await _rateService.DeletePubRate(rateId);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting pub rate! " +
                                     $"Id: {rateId}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/BoardGame")]
        public async Task<IActionResult> AddBoardGames(string id, [FromBody] string[] boardGameIds)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _pubService.AddBoardGames(
                    id,
                    user.Value.Account.Id,
                    boardGameIds);

                return result.HasValue
                    ? OkJson(result.Value)
                    : BadRequest();
            }
            catch (UserNotAllowedToUpdatePubException ex)
            {
                _logger.LogWarning(ex, $"User: {UserEmail} not allowed to add board games to pubId: {id}!");
                return ForbiddenJson();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding board games to pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("{id}/BoardGame")]
        public async Task<IActionResult> GetPubBoardGames(string id, [FromQuery] BoardGameQuery query)
        {
            try
            {
                var specification = _boardGameSpeczilla.GetSpecification(query, id);
                var result = await _boardGameService.GetPaginationResultAsync(specification);

                return result.HasValue
                    ? OkJson(result.Value)
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting board games from pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}/BoardGame")]
        [Authorize]
        public async Task<IActionResult> DeleteBoardGames(string id, [FromBody] string[] boardGameIds)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _pubService.DeleteBoardGames(
                    id,
                    user.Value.Account.Id,
                    boardGameIds);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (UserNotAllowedToUpdatePubException ex)
            {
                _logger.LogWarning(ex, $"User: {UserEmail} not allowed to delete games in pubId: {id}!");
                return ForbiddenJson();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting board games from pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }
    }
}