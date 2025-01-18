
using System.Text;

namespace Animals.API.Middlewares;

public class AnimalMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.Value.Contains("animals") && 
            context.Request.Method.ToUpper().Equals("POST"))
        {
            var body = await context.Request.BodyReader.ReadAsync(); //читаемо тіло реквеста
            string bodyText = Encoding.UTF8.GetString(body.Buffer);
            if (bodyText.Contains("\"Id\""))
            {
                context.Response.StatusCode = 400; //міняємо статус
                await context.Response.BodyWriter.WriteAsync("You cannot add ID"u8.ToArray());
            }
        }
        await next.Invoke(context); //next middleware
    }
}
