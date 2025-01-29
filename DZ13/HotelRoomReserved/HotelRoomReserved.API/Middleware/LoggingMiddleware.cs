namespace HotelRoomReserved.API.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<LoggingMiddleware>>();
        logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
        await next.Invoke(context);
    }
}
