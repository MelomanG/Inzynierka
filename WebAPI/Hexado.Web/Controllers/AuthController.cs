using System;
using System.Threading.Tasks;
using Hexado.Core.Services.Specific;
using Hexado.Web.ActionFilters;
using Hexado.Web.Extensions.Models;
using Hexado.Web.Models;
using Hexado.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILoggerFactory loggerFactory)
        {
            _authService = authService;
            _logger = loggerFactory.CreateLogger<AuthController>();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            try
            {
                var result = await _authService.LoginAsync(model.Email, model.Password);

                return result.HasValue
                    ? OkJson(result.Value.ToTokenResponse())
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
        [ServiceFilter(typeof(AuthorizationHeaderValidation))]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request, [FromHeader] string authorization)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(authorization, request.RefreshToken);

                return result.HasValue
                    ? OkJson(result.Value.ToTokenResponse())
                    : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing token!");
                return InternalServerErrorJson(ex);
            }
        }
    }
}