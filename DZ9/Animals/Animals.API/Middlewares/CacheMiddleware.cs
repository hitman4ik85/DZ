namespace Animals.API.Middlewares;

public class CacheMiddleware : IMiddleware
{
    private static readonly List<(string Path, string Response, DateTime TimesTamp)> Cache = new();
    private static readonly TimeSpan CasheTimeToLive = TimeSpan.FromMinutes(5); // Створюємо об'єкт TimeSpan з інтервалом у 5 хвилин

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method == HttpMethods.Get)
        {
            var cashKey = context.Request.Path.ToString();

            // Видаляємо старі записи
            Cache.RemoveAll(i => DateTime.UtcNow - i.TimesTamp > CasheTimeToLive); // TimesTamp це час, коли елемент був доданий до кешу

            // Перевіряємо чи є в кеші
            var cashedItem = Cache.FirstOrDefault(i => i.Path == cashKey);
            if (!string.IsNullOrEmpty(cashedItem.Response))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cashedItem.Response);
                return;
            }

            // Зберігаємо оригінал
            var originalBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next.Invoke(context);

            // Зберігаємо відповідь
            context.Response.Body.Seek(0, SeekOrigin.Begin); // Повертаємо вказівник на початок потоку, де 0 це початок
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Cache.Add((cashKey, responseBodyText, DateTime.UtcNow));

            // Повертаємо відповідь
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBody);
        }
        else
        {
            await next.Invoke(context);
        }
    }
}
