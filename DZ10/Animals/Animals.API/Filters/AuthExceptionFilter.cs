using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Animals.API.Filters;

public class AuthExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            context.Result = new ContentResult()
            {
                Content = "Authorization header is wrong.",
                StatusCode = 401
            };
        }
        else
        {
            context.Result = new ContentResult()
            {
                Content = "Internal Server Error.",
                StatusCode = 500
            };
        }
    }
}
