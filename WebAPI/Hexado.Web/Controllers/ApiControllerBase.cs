﻿using System;
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

        protected IActionResult CreatedJson(object obj)
        {
            return ToJsonResult(
                obj,
                StatusCodes.Status201Created);
        }

        protected IActionResult OkJson(object obj)
        {
            return ToJsonResult(
                obj,
                StatusCodes.Status200OK);
        }

        protected IActionResult InternalServerErrorJson(Exception ex)
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