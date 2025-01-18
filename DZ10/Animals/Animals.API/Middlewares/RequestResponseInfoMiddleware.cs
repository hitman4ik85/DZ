namespace Animals.API.Middlewares;

public class RequestResponseInfoMiddleware : IMiddleware
{
    private readonly ILogger<RequestResponseInfoMiddleware> _logger;

    public RequestResponseInfoMiddleware(ILogger<RequestResponseInfoMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;
        _logger.LogInformation($"Request: Method = {request.Method}, Path = {request.Path}");

        await next.Invoke(context);

        var response = context.Response;
        _logger.LogInformation($"Response: StatusCode = {response.StatusCode}");
    }
}
