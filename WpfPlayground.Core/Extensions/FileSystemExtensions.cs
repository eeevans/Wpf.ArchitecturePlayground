using System.IO;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>Ensures the folder exists.</summary>
        /// <param name="filePath">The file path.</param>
        public static void EnsureFolderExists(this string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (string.IsNullOrWhiteSpace(directoryName))
                return;
            Directory.CreateDirectory(directoryName);
        }
    }
}
