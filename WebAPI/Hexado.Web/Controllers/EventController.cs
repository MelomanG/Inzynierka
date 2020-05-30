using System;
using System.IO;
using System.Linq;
using Hexado.Core.Services.Specific;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Core.Queries;
using Hexado.Core.Speczillas;
using Hexado.Db.Entities;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class EventController : ApiBaseController
    {
        private readonly IEventService _eventService;
        private readonly IEventSpeczilla _eventSpeczilla;
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger _logger;

        public EventController(
            IEventService eventService,
            IEventSpeczilla eventSpeczilla,
            IHexadoUserService hexadoUserService,
            ILoggerFactory loggerFactory)
        {
            _eventService = eventService;
            _eventSpeczilla = eventSpeczilla;
            _hexadoUserService = hexadoUserService;
            _logger = loggerFactory.CreateLogger<EventController>();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EventModel model)
        {
            try
            {
                var owner = await _hexadoUserService.GetSingleOrMaybeAsync(u =>
                    u.Email == UserEmail);
                if (!owner.HasValue)
                    return Unauthorized();

                var result = await _eventService.CreateAsync(model.ToEntity(owner.Value.Id));
                return result.HasValue
                    ? OkJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating event!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EventQuery query)
        {
            try
            {
                var specification = _eventSpeczilla.GetSpecification(query);
                var result = await _eventService.GetPaginationResultAsync(specification);
                return result.HasValue
                    ? OkJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching events!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(string id)
        {
            try
            {
                var result = await _eventService.GetByIdAsync(id);

                if (!result.HasValue)
                    return NotFound();

                var eventResponse = result.Value.ToResponse();

                if (!string.IsNullOrWhiteSpace(UserEmail))
                {
                    var participatedEvents = await _hexadoUserService.GetUserParticipatedEvents(UserEmail);
                    if (participatedEvents.HasValue)
                        if (participatedEvents.Value.Any(lbg => lbg.Id == eventResponse.Id))
                            eventResponse.IsUserParticipant = true;

                    var ownedEvents = await _hexadoUserService.GetUserOwnedEvents(UserEmail);
                    if (ownedEvents.HasValue)
                        if (ownedEvents.Value.Any(lbg => lbg.Id == eventResponse.Id))
                            eventResponse.IsUserEvent = true;

                    var likedPubs = _hexadoUserService.GetLikedPubs(UserEmail);
                    if (likedPubs.HasValue)
                        if (likedPubs.Value.Any(lp => lp.Id == eventResponse.Pub.Id))
                            eventResponse.Pub.IsLikedByUser = true;

                    var likedBoardGames = _hexadoUserService.GetLikedBoardGames(UserEmail);
                    if (likedBoardGames.HasValue)
                        if (likedBoardGames.Value.Any(lp => lp.Id == eventResponse.BoardGame.Id))
                            eventResponse.BoardGame.IsLikedByUser = true;
                }

                return OkJson(eventResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching event!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserEvents()
        {
            try
            {
                var result = await _hexadoUserService.GetUserEvents(UserEmail);
                return result.HasValue
                    ? OkJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching events!");
                return InternalServerErrorJson(ex);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(string id, EventModel model)
        {
            try
            {
                var user = await _hexadoUserService.GetSingleOrMaybeAsync(u => u.Email == UserEmail);
                if (!user.HasValue)
                    return Unauthorized();

                var result = await _eventService.UpdateAsync(id, model.ToEntity(user.Value.Id));
                return result.HasValue
                    ? OkJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching event!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _eventService.DeleteByIdAsync(id);

                var notFullPath = Path.Combine("Images", "Events");
                var fromRootPath = Path.Combine(Directory.GetCurrentDirectory(), notFullPath);
                ValidateIfStaticFileExists(id, fromRootPath);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting event!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/participant/{participantId}")]
        [Authorize]
        public async Task<IActionResult> AddParticipant(string id, string participantId)
        {
            try
            {
                var result = await _eventService.AddParticipantAsync(
                    id,
                    participantId);

                return result.HasValue
                    ? OkJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding board games to pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}/participant/{participantId}")]
        [Authorize]
        public async Task<IActionResult> DeleteParticipant(string id, string participantId)
        {
            try
            {
                var result = await _eventService.DeleteParticipantAsync(
                    id,
                    participantId);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting board games from pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/join")]
        [Authorize]
        public async Task<IActionResult> JoinEvent(string id)
        {
            try
            {
                var user = (await _hexadoUserService.GetSingleOrMaybeAsync(hu => hu.Email == UserEmail)).Value;
                var result = await _eventService.AddParticipantAsync(
                    id,
                    user.Id);

                return result.HasValue
                    ? OkJson(result.Value.ToResponse())
                    : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding board games to pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("{id}/quit")]
        [Authorize]
        public async Task<IActionResult> QuitEvent(string id)
        {
            try
            {
                var user = (await _hexadoUserService.GetSingleOrMaybeAsync(hu => hu.Email == UserEmail)).Value;
                var result = await _eventService.DeleteParticipantAsync(
                    id,
                    user.Id);

                return result.HasValue
                    ? OkJson(result.Value)
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting board games from pub! " +
                                     $"PubId: {id}");
                return InternalServerErrorJson(ex);
            }
        }


        [HttpGet("cities")]
        public IActionResult GetCities()
        {
            try
            {
                var result = _eventService.GetCities();

                if (!result.HasValue)
                    return NotFound();

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching cities!");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadFile(string id, IFormFile image)
        {
            try
            {
                var notFullPath = Path.Combine("Images", "Events");
                var fromRootPath = Path.Combine(Directory.GetCurrentDirectory(), notFullPath);
                ValidateIfStaticFileExists(id, fromRootPath);

                var imageName = id + image.FileName;
                var fullRootPath = Path.Combine(fromRootPath, imageName);
                var staticFilePath = Path.Combine(notFullPath, imageName);
                var result = Maybe<Event>.Nothing;
                await using (var stream = new FileStream(fullRootPath, FileMode.Create))
                {
                    image.CopyTo(stream);
                    result = await _eventService.SetImagePath(id, staticFilePath);
                }

                if (!result.HasValue)
                    return NotFound();
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading image for event! " +
                                     $"Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        //[HttpDelete("clear")]
        //public async Task<IActionResult> Clear()
        //{
        //    await _eventService.ClearAsync();
        //    return Ok();
        //}
    }
}
