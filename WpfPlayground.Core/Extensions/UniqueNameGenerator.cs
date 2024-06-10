using System;
using System.Linq;
using WpfPlayground.Core.Exceptions;

namespace WpfPlayground.Core.Extensions
{
    public interface IUniqueNameGenerator
    {
        string GenerateUniqueName(string sourceName, string[] existingNames, int maxLength);
    }

    public class UniqueNameGenerator : IUniqueNameGenerator
    {
        private readonly ICopyBaseNameExtractor _baseNameExtractor;

        public UniqueNameGenerator(ICopyBaseNameExtractor baseNameExtractor)
        {
            _baseNameExtractor = baseNameExtractor;
        }

        public string GenerateUniqueName(string sourceName, string[] existingNames, int maxLength)
        {
            if (maxLength < 12)
                throw new WpfPlaygroundException("MaxLength must be greater than or equal to 12");

            if (string.IsNullOrEmpty(sourceName))
                throw new WpfPlaygroundException("Null or empty names are not supported");

            if (!existingNames.Contains(sourceName, StringComparer.OrdinalIgnoreCase))
                return sourceName;

            const string copy = "-Copy";
            var baseName = _baseNameExtractor.GetBaseName(sourceName);

            string newName = null;

            var newLength = baseName.Length + copy.Length + 4;  // accommodate up to -Copy9999
            if (newLength > maxLength)
            {
                var overage = newLength - maxLength;
                newName = $"{baseName.Substring(0, baseName.Length - overage)}{copy}";
            }

            newName = newName ?? $"{baseName}{copy}";

            if (NameAlreadyExists(existingNames, newName))
            {
                var workingGap = maxLength - newName.Length;
                var maxIterator = int.Parse(new string('9', Math.Min(4, workingGap)));
                for (var i = 2; i <= maxIterator; i++)
                {
                    var workingName = $"{newName}{i}";
                    if (NameAlreadyExists(existingNames, workingName)) continue;
                    newName = workingName;
                    break;
                }

                if (NameAlreadyExists(existingNames, newName))
                    // ran out of unique names
                    throw new WpfPlaygroundException("Ran out of unique names");
            }

            return newName;
        }

        private static bool NameAlreadyExists(string[] existingNames, string newName)
        {
            return existingNames.Contains(newName);
        }
    }
}
