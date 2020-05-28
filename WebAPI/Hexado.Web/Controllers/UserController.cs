using System;
using System.Linq;
using System.Threading.Tasks;
using Hexado.Core.Services.Specific;
using Hexado.Web.Extensions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiBaseController
    {
        private readonly IContactService _contactService;
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IContactService contactService,
            IHexadoUserService hexadoUserService,
            ILoggerFactory loggerFactory)
        {
            _contactService = contactService;
            _hexadoUserService = hexadoUserService;
            _logger = loggerFactory.CreateLogger<UserController>();
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> SearchUsers([FromQuery] string? search)
        {
            try
            {
                var result = await _hexadoUserService.Search(search ?? string.Empty);
                return result.HasValue
                    ? OkJson(result.Value.OrderBy(u => u.UserName).ToResponse())
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching users. Search: {search}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("{id}/contact")]
        [Authorize]
        public async Task<IActionResult> AddContact(string id)
        {
            try
            {
                await _contactService.AddContactAsync(id, UserEmail);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while adding contact. id: {id}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpGet("contacts")]
        [Authorize]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var result = await _contactService.GetUserContacts(UserEmail);

                return result.HasValue
                    ? OkJson(result.Value.ToResponse().OrderBy(c => c.ContactUserName))
                    : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching contacts.");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpDelete("contact/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteContact(string id)
        {
            try
            {
                await _contactService.DeleteContact(id, UserEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting contact. Id: {id}");
                return InternalServerErrorJson(ex);
            }
        }
    }
}