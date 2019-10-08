using Hexado.Core.Constants;
using Hexado.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hexado.Web.ActionFilters
{
    public class AuthorizationHeaderValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(HeaderKey.Authorization, out var value))
                context.Result = new UnauthorizedResult();

            var accessToken = value.ToString();
            if (string.IsNullOrWhiteSpace(accessToken) || !accessToken.Contains(ConstantKey.BearerWithSpace))
                context.Result = new UnauthorizedResult();
        }
    }
}