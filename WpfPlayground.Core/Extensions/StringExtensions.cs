using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string XorIt(this string key, string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < input.Length; ++index)
                stringBuilder.Append((char)((uint)input[index] ^ (uint)key[index % key.Length]));
            return stringBuilder.ToString();
        }

        /// <summary>Equalses the ignoring case.</summary>
        /// <param name="s">The s.</param>
        /// <param name="comparedValue">The compared value.</param>
        /// <returns></returns>
        public static bool EqualsIgnoringCase(this string s, string comparedValue)
        {
            return s.Equals(comparedValue, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>Formats the with.</summary>
        /// <param name="s">The s.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>Ifs the empty.</summary>
        /// <param name="s">The s.</param>
        /// <param name="action">The action.</param>
        public static void IfEmpty(this string s, Action action)
        {
            if (!string.IsNullOrEmpty(s))
                return;
            action();
        }

        /// <summary>Ifs the not empty.</summary>
        /// <param name="s">The s.</param>
        /// <param name="action">The action.</param>
        public static void IfNotEmpty(this string s, Action<string> action)
        {
            if (string.IsNullOrEmpty(s))
                return;
            action(s);
        }

        /// <summary>True if string is null or empty</summary>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>True if string is not null or empty</summary>
        public static bool IsNotEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        /// <summary>True if string is not null or empty</summary>
        public static bool ContainsText(this string s)
        {
            if (!string.IsNullOrEmpty(s))
                return !string.IsNullOrWhiteSpace(s);
            return false;
        }

        /// <summary>Matcheses the specified pattern.</summary>
        /// <param name="s">The s.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static bool Matches(this string s, string pattern)
        {
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>Ases the date.</summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static DateTime? AsDate(this string s)
        {
            DateTime result;
            if (!DateTime.TryParse(s, out result))
                return new DateTime?();
            return new DateTime?(result);
        }

        /// <summary>Joins the specified seperator.</summary>
        /// <param name="strings">The strings.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> strings, string seperator)
        {
            return string.Join(seperator, strings.ToArray<string>());
        }

        /// <summary>Splits the on capital letters.</summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string[] SplitOnCapitalLetters(this string input)
        {
            List<StringBuilder> source = new List<StringBuilder>();
            source.Add(new StringBuilder());
            char c1 = char.MinValue;
            foreach (char c2 in input)
            {
                if (char.IsLower(c1) && char.IsUpper(c2))
                    source.Add(new StringBuilder());
                source.Last<StringBuilder>().Append(c2);
                c1 = c2;
            }
            return source.Select<StringBuilder, string>((Func<StringBuilder, string>)(sb => sb.ToString())).ToArray<string>();
        }

        /// <summary>Lowers the case underscore.</summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string LowerCaseUnderscore(this string input)
        {
            return string.Join("_", input.Replace(" ", "_").SplitOnCapitalLetters()).ToLower();
        }

        /// <summary>Nulls the or empty.</summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static bool NullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>Strips the spaces.</summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string StripSpaces(this string s)
        {
            return s.Replace(" ", "");
        }

        /// <summary>Use the current thread's culture info for conversion</summary>
        public static string ToTitleCase(this string str)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            return str.ToTitleCase(currentCulture);
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            CultureInfo cultureInfo = new CultureInfo(cultureInfoName);
            return str.ToTitleCase(cultureInfo);
        }

        /// <summary>Overload which uses the specified culture info</summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>To the pascal case.</summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ToPascalCase(this string source)
        {
            return string.Concat(((IEnumerable<string>)source.Split(' ')).Select<string, string>((Func<string, int, string>)((x, index) =>
            {
                if (index <= 0)
                    return x;
                return x.ToTitleCase();
            })));
        }

        /// <summary>Ases the title.</summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string AsTitle(this string source)
        {
            return string.Join(" ", source.SplitOnCapitalLetters());
        }

        /// <summary>Separates the words.</summary>
        /// <param name="camelCased">The camel cased.</param>
        /// <returns></returns>
        public static string SeparateWords(this string camelCased)
        {
            return string.Join(" ", new Regex("  (?<=[A-Z])(?=[A-Z][a-z])    # UC before me, UC lc after me\r\n                 |  (?<=[^A-Z])(?=[A-Z])        # Not UC before me, UC after me\r\n                 |  (?<=[A-Za-z])(?=[^A-Za-z])  # Letter before me, non letter after me\r\n                 ", RegexOptions.IgnorePatternWhitespace).Split(camelCased));
        }

        /// <summary>Withes the timestamp.</summary>
        /// <param name="path">The path.</param>
        /// <param name="currentDateTime">The current date time.</param>
        /// <returns></returns>
        public static string WithTimestamp(this string path, ICurrentDateTime currentDateTime)
        {
            string directoryName = Path.GetDirectoryName(path);
            string withoutExtension = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            string str = currentDateTime.Now().ToString("yyyyMMddHHmmss");
            return "{0}_{1}{2}".FormatWith((object)Path.Combine(directoryName, withoutExtension ?? ""), (object)str, (object)extension);
        }

        /// <summary>Truncates the specified maximum length.</summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentException">'maxLength' must be greater than 0;maxLength</exception>
        public static string Truncate(this string value, int maxLength)
        {
            if (maxLength <= 0)
                throw new ArgumentException("'maxLength' must be greater than 0", nameof(maxLength));
            if (value == null)
                return (string)null;
            if (value.Length > maxLength)
                return value.Substring(0, maxLength);
            return value;
        }

        /// <summary>
        /// Replaces the content of a capture group, instead of the full match.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="pattern">The regex containing a named capture group. (Ex: (?{value}true|false))</param>
        /// <param name="captureGroup">The name of the capture group. (Ex: (?{value}true|false))</param>
        /// <param name="replacement">The text to use as a replacement for the text in the capture group.</param>
        /// <param name="options">RegexOptions to be used for the operations. IgnoreCase and IgnoreWhitespace are respected when comparing capture text to the replacement value.</param>
        /// <returns>The modified string.</returns>
        /// <remarks>
        /// Regex replace can be used for search and replace operations where you wish to replace the contents of a capture group, instead of the full match.
        /// Capture group syntax:  (?{capture_group_name}text)
        /// "value" is the capture group name. "true" or "false" is the text being captured.
        /// (Note that Summary comments in .NET use mustache brackets in place of angle brackets.)
        /// </remarks>
        /// <example>
        /// var input = "This is a test.";
        /// var regex = @"This (?{value}is the|was the|is a|could be a) test";
        /// var groupname = "value";
        /// var replacement = "is the worst";
        /// var result = input.RegexReplace( regex, groupname, replacement );
        /// Console.WriteLine( "New Value: {1}", success, result );
        /// Assert.Equal( "This is the worst test.", result );
        /// Assert.True( success );
        /// 
        /// Should Result in:
        /// New Value: This is the worst test.
        /// </example>
        public static string RegexReplace(
          this string text,
          string pattern,
          string captureGroup,
          string replacement,
          RegexOptions options = RegexOptions.IgnoreCase)
        {
            string result;
            text.RegexReplace(pattern, captureGroup, replacement, out result, options);
            return result;
        }

        /// <summary>
        /// Replaces the content of a capture group, instead of the full match.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="pattern">The regex containing a named capture group. (Ex: (?{value}true|false))</param>
        /// <param name="captureGroup">The name of the capture group. (Ex: (?{value}true|false))</param>
        /// <param name="replacement">The text to use as a replacement for the text in the capture group.</param>
        /// <param name="result">The modified string after replacing all named capture groups.</param>
        /// <param name="options">RegexOptions to be used for the operations. IgnoreCase and IgnoreWhitespace are respected when comparing capture text to the replacement value.</param>
        /// <returns>True, if the pattern was matched.</returns>
        /// <remarks>
        /// Regex replace can be used for search and replace operations where you wish to replace the contents of a capture group, instead of the full match.
        /// Capture group syntax:  (?{capture_group_name}text)
        /// "value" is the capture group name. "true" or "false" is the text being captured.
        /// (Note that Summary comments in .NET use mustache brackets in place of angle brackets.)
        /// </remarks>
        /// <example>
        /// string result;
        /// var input = "This is a test.";
        /// var regex = @"This (?{value}is the|was the|is a|could be a) test";
        /// var groupname = "value";
        /// var replacement = "is the worst";
        /// var success = input.RegexReplace( regex, groupname, replacement, out result );
        /// Console.WriteLine( "Match Success: {0}\nNew Value: {1}", success, result );
        /// Assert.Equal( "This is the worst test.", result );
        /// Assert.True( success );
        /// 
        /// Should Result in:
        /// Match Success: true
        /// New Value: This is the worst test.
        /// </example>
        public static bool RegexReplace(
          this string text,
          string pattern,
          string captureGroup,
          string replacement,
          out string result,
          RegexOptions options = RegexOptions.IgnoreCase)
        {
            MatchCollection matchCollection = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            result = text;
            if (matchCollection.Count == 0)
                return false;
            int startIndex = 0;
            StringBuilder stringBuilder = new StringBuilder(text.Length);
            foreach (Match match in matchCollection)
            {
                Group group = match.Groups[captureGroup];
                string str1 = text.Substring(startIndex, group.Index);
                stringBuilder.Append(str1);
                int num = (options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase ? 1 : 0;
                bool flag = (options & RegexOptions.IgnorePatternWhitespace) == RegexOptions.IgnorePatternWhitespace;
                string str2 = group.Value;
                string str3 = replacement;
                if (num != 0)
                {
                    str2 = str2.ToLower();
                    str3 = str3.ToLower();
                }
                if (flag)
                {
                    str2 = str2.Trim();
                    str3 = str3.Trim();
                }
                stringBuilder.Append(str2 == str3 ? group.Value : replacement);
                startIndex = group.Index + group.Length;
            }
            if (startIndex < text.Length)
                stringBuilder.Append(text.Substring(startIndex));
            result = stringBuilder.ToString();
            return true;
        }
    }

    public interface ICurrentDateTime
    {
        DateTime Now();
    }
}