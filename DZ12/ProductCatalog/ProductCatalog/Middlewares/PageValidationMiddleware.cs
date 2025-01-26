namespace ProductCatalog.API.Middlewares;

public class PageValidationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var query = context.Request.Query;
        if (query.ContainsKey("page") && int.TryParse(query["page"], out int page) && page < 0)
        {
            throw new ArgumentException("Page cannot be less than 0.");
        }

        if (query.ContainsKey("pageSize") && int.TryParse(query["pageSize"], out int pageSize) && pageSize <= 0)
        {
            throw new ArgumentException("PageSize must be greater than 0.");
        }

        await next.Invoke(context);
    }
}
