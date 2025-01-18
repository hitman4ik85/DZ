using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.CompilerServices;
using System.Text;

namespace Animals.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class LogFilter : Attribute, IActionFilter
{
    private string _name;

    public LogFilter([CallerMemberName] string? name = null)
    {
        _name = name;
    }

    //до нашого ендпоінта
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LogFilter>>();
        logger.LogInformation($"Before Filter: {_name}");
        logger.LogInformation($"Controller: {context.Controller}; Path: {context.HttpContext.Request.Path}");
    }

    //після нашого ендпоінта
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LogFilter>>();
        logger.LogInformation($"After Filter: {_name}");
        logger.LogInformation($"{context.HttpContext.Response.StatusCode}");
    }

}
