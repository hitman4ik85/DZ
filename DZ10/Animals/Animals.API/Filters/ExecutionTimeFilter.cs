using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Animals.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class ExecutionTimeFilter : Attribute, IActionFilter
{
    private readonly Stopwatch _stopwatch;
    private string _name;

    public ExecutionTimeFilter([CallerMemberName] string? name = null)
    {
        _stopwatch = new Stopwatch();
        _name = name;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ExecutionTimeFilter>>();
        logger.LogInformation($"Endpoint '{_name}' started.");
        _stopwatch.Start();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ExecutionTimeFilter>>();
        _stopwatch.Stop();
        logger.LogInformation($"Endpoint '{_name}' ended.");
        logger.LogInformation($"Total time for '{_name}' = {_stopwatch.ElapsedMilliseconds}ms");
    }
}
