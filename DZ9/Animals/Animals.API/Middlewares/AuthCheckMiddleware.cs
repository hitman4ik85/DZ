namespace Animals.API.Middlewares;

public class AuthCheckMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method != HttpMethods.Get 
            && !context.Request.Headers.ContainsKey("Auth"))
        {
            context.Response.StatusCode = 401; // Ne Avtoruzovano
            await context.Response.WriteAsync("Unauthorized: Missing Auth header");
            return;
        }

        await next.Invoke(context);
    }
}
