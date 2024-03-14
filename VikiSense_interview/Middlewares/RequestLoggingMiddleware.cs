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
        var logMsg = $"Request date/time=> Timestamp:{DateTime.Now.ToLocalTime()} \n" +
                $"Method=> {request.Method} \n " +
                $"Path=> {request.Path} \n " +
                $"Query params=> {GetQueryParams(request.Query)} \n " +
                $"Headers=> {GetHeaders(request.Headers)}";
        logger.LogInformation(logMsg );
        await _next(context);
    }
     


    private static string GetHeaders(IHeaderDictionary headersDic)
    {
        var headers = "";
        headersDic
            .ToList()
            .ForEach(x =>
              {
                headers += x.Key + "=" + x.Value + "; ";
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
                 queryParams += x.Key + "=" + x.Value + "; ";
               });
        return queryParams;
    }   
}



