using System.Net;
using VikiSense_interview.DTOs;

namespace VikiSense_interview.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Error encountered.");



        ExceptionResponse response = exception switch
        {
            IndexOutOfRangeException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "The specified index is not valid."),
            ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
            KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The requested key not found."),
            UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Error encountered while processing your request. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode =  (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(response);
    }
}
