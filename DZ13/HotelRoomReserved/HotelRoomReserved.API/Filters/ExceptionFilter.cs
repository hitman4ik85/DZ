using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace HotelRoomReserved.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest, // 400 Bad Request
            KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404 Not Found
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401 Unauthorized
            _ => (int)HttpStatusCode.InternalServerError // 500 Internal Server Error
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
