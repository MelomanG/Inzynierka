using System;
using System.Threading.Tasks;
using Hexado.Core.Services;
using Hexado.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ApiBaseController
    {
        private readonly IHexadoUserService _hexadoUserService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IHexadoUserService hexadoUserService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _hexadoUserService = hexadoUserService;
            _logger = loggerFactory.CreateLogger<AuthController>();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            try
            {
                var result = await _hexadoUserService.LoginAsync(model.Email, model.Password);

                return result.HasValue
                    ? OkJson(result.Value)
                    : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login! " +
                                     $"User: {model.Email}");
                return InternalServerErrorJson(ex);
            }
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            try
            {
                var result = await _hexadoUserService.RefreshTokenAsync(UserEmail, request.RefreshToken);

                return result.HasValue
                    ? OkJson(result.Value)
                    : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing token! " +
                                     $"User: {UserEmail ?? "unknown"}");
                return InternalServerErrorJson(ex);
            }
        }
    }
}