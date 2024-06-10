using System.Text.RegularExpressions;
using WpfPlayground.Core.Exceptions;

namespace WpfPlayground.Core.Extensions
{
    public interface ICopyBaseNameExtractor
    {
        string GetBaseName(string sourceName);
    }

    public class CopyBaseNameExtractor : ICopyBaseNameExtractor
    {
        public string GetBaseName(string sourceName)
        {
            var regex = new Regex(@"(.+)-Copy\d{0,4}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            var match = regex.Match(sourceName);
            if (match.Success)
            {
                if (match.Groups.Count < 2) throw new WpfPlaygroundException($"Regex error in GetBaseName for: {sourceName}");
                var baseName = match.Groups[1].Value;
                return baseName;
            }
            return sourceName;
        }
    }
}