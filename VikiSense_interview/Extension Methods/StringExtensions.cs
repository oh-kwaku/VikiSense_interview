using System.IO.Compression;

namespace VikiSense_interview.Extension_Methods
{
    public static class StringExtensions
    {

        /// <summary>
        /// Converts string to compression level. ie.  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CompressionLevel ToCompressionLevel(this string value) 
        {
            if (!Enum.TryParse<CompressionLevel>(value, out CompressionLevel result))
            {
                return CompressionLevel.Optimal;
            }
            return result;
        }
    }
}
