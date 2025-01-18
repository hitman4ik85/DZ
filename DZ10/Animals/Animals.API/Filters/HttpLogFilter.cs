using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Animals.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class HttpLogFilter : Attribute, IActionFilter
{
    private string _name;

    public HttpLogFilter([CallerMemberName] string? name = null)
    {
        _name = name;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HttpLogFilter>>();
        logger.LogInformation($"Filter started: {_name}");
        logger.LogInformation($"HTTP Method: {context.HttpContext.Request.Method}");

        if (context.HttpContext.Request.Method != "GET")
        {
            logger.LogWarning("Invalid HTTP method. Only GET is allowed.");
            context.Result = new BadRequestObjectResult("Invalid HTTP method. Only GET requests are allowed.");
        }
    }

    public async void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HttpLogFilter>>();
        logger.LogInformation($"Filter completed: {_name}");
        logger.LogInformation($"Response Code: {context.HttpContext.Response.StatusCode}");
    }
}
