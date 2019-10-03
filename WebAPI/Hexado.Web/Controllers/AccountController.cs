using System;
using System.Threading.Tasks;
using Hexado.Core.Services;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IHexadoUserService hexadoUserService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _hexadoUserService = hexadoUserService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            try
            {
                var result = await _hexadoUserService.CreateAsync(
                    model.ToHexadoUser(),
                    model.Password);

                return result.Succeeded
                    ? CreatedJson(new { model.Username })
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration!" +
                                     $"{model.Email}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            try
            {
                var result = await _hexadoUserService.Login(model.Email, model.Password);

                return result.IsValid
                    ? OkJson(new { result.Token })
                    : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login!" +
                                     $"User: {model.Email}");
                return InternalServerErrorJson(ex);
            }
        }
    }
}