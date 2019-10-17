using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hexado.Web.Controllers
{
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected string UserEmail =>  HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;

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

        protected static IActionResult ForbiddenJson()
        {
            return ToJsonResult(
                default,
                StatusCodes.Status200OK);
        }

        
        protected static IActionResult InternalServerErrorJson(Exception ex)
        {
            return ToJsonResult(
                new { errorMessage = ex.Message },
                StatusCodes.Status500InternalServerError);
        }

        private static JsonResult ToJsonResult(object? obj, int statusCode)
        {
            return new JsonResult(obj)
            {
                StatusCode = statusCode
            };
        }
    }
}