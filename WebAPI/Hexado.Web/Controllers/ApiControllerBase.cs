using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hexado.Web.Controllers
{

    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly ILogger<ApiControllerBase> _logger;

        protected ApiControllerBase(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiControllerBase>();
        }

        protected IActionResult Created(object obj)
        {
            return new JsonResult(obj)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}