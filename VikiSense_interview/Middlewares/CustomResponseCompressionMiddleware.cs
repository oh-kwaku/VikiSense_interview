using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using VikiSense_interview.Extension_Methods;

namespace VikiSense_interview.Middlewares;
public class CustomResponseCompressionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _compressionThreshold;
    private readonly string _compressionLevel;

    public CustomResponseCompressionMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _compressionThreshold = config.GetValue<int>("CompressionThreshold");
        _compressionLevel = config.GetValue<string>("CompressionLevel");
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await _next(context);

            if (IsContentTypeSupported(context.Response) && ShouldCompress(context.Response))
            {
                context.Response.Headers[HeaderNames.ContentEncoding] = "gzip";

                memoryStream.Seek(0, SeekOrigin.Begin);

                var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString().ToLower();

                using (var compressedStream = new MemoryStream())
                {
                    if (acceptEncoding.Contains("gzip"))
                    {
                        using (var gzipStream = new GZipStream(compressedStream, _compressionLevel.ToCompressionLevel(), true))
                        {
                            await memoryStream.CopyToAsync(gzipStream);
                            gzipStream.Close();
                            compressedStream.Seek(0, SeekOrigin.Begin);

                            // Copy compressed data back to the original response body stream
                            await compressedStream.CopyToAsync(originalBodyStream);
                        }
                    }
                    else if (acceptEncoding.Contains("br"))
                    {
                        using (var brotliStream = new BrotliStream(compressedStream, _compressionLevel.ToCompressionLevel(), true))
                        {
                            await memoryStream.CopyToAsync(brotliStream);
                            brotliStream.Close();
                            compressedStream.Seek(0, SeekOrigin.Begin);

                            // Copy compressed data back to the original response body stream
                            await compressedStream.CopyToAsync(originalBodyStream);
                        }
                    }


                }
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }

    private bool IsContentTypeSupported(HttpResponse response)
    {
        var supportedContentTypes = new List<string>
        {
            "application/json",
            "application/xml"
        };
        var contentType = response.ContentType?.ToLower();
        return contentType != null && supportedContentTypes.Any(x => contentType.Contains(x));
    }

    private bool ShouldCompress(HttpResponse response)
    {
        return response.Body.Length >= _compressionThreshold;
    }
}