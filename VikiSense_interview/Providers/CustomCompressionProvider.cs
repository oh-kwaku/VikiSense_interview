using Microsoft.AspNetCore.ResponseCompression;


using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace VikiSense_interview.Providers;
public class CustomCompressionProvider : ICompressionProvider
{
    public string EncodingName => "custom";

    public bool SupportsFlush { get; }

    public CustomCompressionProvider()
    {
      
    }
    public Stream CreateStream(Stream outputStream)
    {
        var memoryStream = new MemoryStream();
        outputStream.CopyTo(memoryStream);

        // Check if the response size is greater than the minimum threshold
        if (memoryStream.Length < 1024)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        var compressedStream = new MemoryStream();
        using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, leaveOpen: true))
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
              memoryStream.CopyTo(gzipStream);
        }
        compressedStream.Seek(0, SeekOrigin.Begin);
        return compressedStream;
    }
}
