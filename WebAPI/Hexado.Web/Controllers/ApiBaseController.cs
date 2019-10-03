using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        private readonly ILogger<ApiBaseController> _logger;

        protected string UserEmail =>  HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.ToString();

        protected ApiBaseController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiBaseController>();
        }

        protected static IActionResult CreatedJson(object obj)
        {
            return ToJsonResult(
                obj,
                StatusCodes.Status201Created);
        }

        protected static IActionResult OkJson(object obj)
        {
            return ToJsonResult(
                obj,
                StatusCodes.Status200OK);
        }

        protected static IActionResult InternalServerErrorJson(Exception ex)
        {
            return ToJsonResult(
                new { errorMessage = ex.Message },
                StatusCodes.Status500InternalServerError);
        }

        private static JsonResult ToJsonResult(object obj, int statusCode)
        {
            return new JsonResult(obj)
            {
                StatusCode = statusCode
            };
        }
    }
}