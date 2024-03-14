using System.Linq;
namespace VikiSense_interview.Middlewares;

public class RequestLoggingMiddleware
{

    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context, ILogger<RequestLoggingMiddleware> logger)
    {
        var request = context.Request;

        logger.LogInformation(
                $"Request=>Timestamp:{DateTime.Now.ToLocalTime()} |" +
                $" Method: {request.Method} | " +
                $" Path: {request.Path} | " +
                $"Query params:{GetQueryParams(request.Query)} | " +
                $"Headers:{GetHeaders(request.Headers)}"
                );
        await _next(context);
    }
     


    private static string GetHeaders(IHeaderDictionary headersDic)
    {
        var headers = "";
        headersDic
            .ToList()
            .ForEach(x =>
              {
                headers += x.Key + "=" + x.Value + ";";
              });
        return headers;
    }  
    private static string GetQueryParams(IQueryCollection query)
    {
        var queryParams = "";
        query
            .ToList()
            .ForEach(x =>
               {
                 queryParams += x.Key + "=" + x.Value + ";";
               });
        return queryParams;
    }   
}



