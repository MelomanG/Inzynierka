using System;
using System.Threading.Tasks;
using Hexado.Core.Queries;
using Hexado.Core.Services;
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
        private readonly IRateService _rateService;
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger _logger;

        public PubController(
            IPubService pubService,
            IPubSpeczilla pubSpeczilla,
            IRateService rateService,
            IHexadoUserService hexadoUserService,
            ILoggerFactory loggerFactory)
        {
            _pubService = pubService;
            _pubSpeczilla = pubSpeczilla;
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
                //TODO define query. Consider splitting Query to query params and body 
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
                //TODO Check ownership
                var result = await _pubService.UpdateAsync(model.ToEntity(id));

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
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
                //TODO Check ownership
                var result = await _pubService.DeleteByIdAsync(id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting pub! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/rate")]
        [Authorize]
        public async Task<IActionResult> RateBoardGame(string id, RateModel model)
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

        [HttpPut("{id}/rate/{rateId}")]
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

        [HttpDelete("rate/{rateId}")]
        [Authorize]
        public async Task<IActionResult> DeleteBoardGameRate(string rateId)
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
    }
}