using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace VikiSense_interview.Middlewares;

public class CustomResponseCompressionMiddleware
{
    
    private readonly RequestDelegate _next;
    private readonly int _compressionThreshold;
   
    public CustomResponseCompressionMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _compressionThreshold = config.GetValue<int>("CompressionThreshold");
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;


            memoryStream.Seek(0, SeekOrigin.Begin);

            var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString().ToLower();

            if (ShouldCompress(context.Response, acceptEncoding))
            {
                await CompressResponse(memoryStream, originalBodyStream, context, acceptEncoding);
            }
            else
            {
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
            await _next(context);
    }

    private bool ShouldCompress(HttpResponse response, string acceptEncoding)
    {
        return response.Body.Length >= _compressionThreshold && (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("br"));
    }

    private async Task CompressResponse(Stream input, Stream output, HttpContext context, string acceptEncoding)
    {
        if (acceptEncoding.Contains("br"))
        {
            using (var brotli = new BrotliStream(output, CompressionMode.Compress))
            {
                await input.CopyToAsync(brotli);
            }
            context.Response.Headers.Add("Content-Encoding", "br");
        }
        else if (acceptEncoding.Contains("gzip"))
        {
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                await input.CopyToAsync(gzip);
            }
            context.Response.Headers.Add("Content-Encoding", "application/json; charset=utf-8");
        }
    }
}

public static class ResponseCompressionMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseCompression(this IApplicationBuilder builder, int compressionThreshold = 1024)
    {
        return builder.UseMiddleware<ResponseCompressionMiddleware>(compressionThreshold);
    }
}
