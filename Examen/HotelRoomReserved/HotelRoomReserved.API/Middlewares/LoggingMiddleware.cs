using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HotelRoomReserved.API.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<LoggingMiddleware>>();

        // Логування вхідного запиту
        string requestBodyText = string.Empty;

        if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            requestBodyText = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        logger.LogInformation($"{DateTime.UtcNow:HH:mm:ss} | [{context.Request.Method}] {context.Request.Path} : {requestBodyText}");

        // Передача керування наступному middleware
        await next(context);

        // Логування вихідної відповіді
        logger.LogInformation($"{DateTime.UtcNow:HH:mm:ss} | {context.Request.Path} [{context.Response.StatusCode}]");
    }
}
