using VikiSense_interview.Middlewares;

namespace VikiSense_interview.Extension_Methods;
public static class MiddlewareExtensions
{
    /// <summary>
    ///custom Middleware to log the request details
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }

    /// <summary>
    /// Custom Middleware to handle errors
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// Custom middleware to compress response
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomResponseCompressionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomResponseCompressionMiddleware>();
    }
}
