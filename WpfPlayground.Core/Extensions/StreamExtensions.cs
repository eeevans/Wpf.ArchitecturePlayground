using System.IO;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>Gets the data.</summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static byte[] GetData(this Stream input)
        {
            MemoryStream memoryStream = new MemoryStream();
            input.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
