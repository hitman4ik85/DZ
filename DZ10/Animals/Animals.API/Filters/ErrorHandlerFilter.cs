using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Animals.API.Filters;

public class ErrorHandlerFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            ArgumentException => 400,
            NullReferenceException => 404,
            UnauthorizedAccessException => 401,
            _ => 500
        };

        context.Result = new ContentResult()
        {
            Content = context.Exception.Message,
            StatusCode = statusCode
        };
        
    }
}
