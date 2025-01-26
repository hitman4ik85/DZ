using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductCatalog.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            ArgumentException => 400, // Bad Request
            KeyNotFoundException => 404, // Not Found
            UnauthorizedAccessException => 401, // Unauthorized
            _ => 500 // Internal Server Error
        };

        context.Result = new ObjectResult(new
        {
            Error = context.Exception.Message,
            Type = context.Exception.GetType().Name
        })
        {
            StatusCode = statusCode
        };
    }
}
