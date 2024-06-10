using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionExtensions
    {
        private static readonly IEnumerable<int> _specialSqlExceptionCodes = new int[8]
        {
      -1,
      53,
      4060,
      1088,
      208,
      2,
      10060,
      10061
        };

        /// <summary>Parses the inner exceptions.</summary>
        /// <param name="exc">The exc.</param>
        /// <param name="indent">The indent.</param>
        /// <returns></returns>
        public static string ParseInnerExceptions(this Exception exc, string indent)
        {
            if (exc == null || exc.InnerException == null)
                return "";
            return "{0}Inner: {1}{2}{3}".FormatWith((object)indent, (object)exc.InnerException.Message, (object)Environment.NewLine, (object)exc.InnerException.ParseInnerExceptions(indent + "   "));
        }

        /// <summary>To the stack trace.</summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string ToStackTrace(this Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Exception");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Exception Message: " + ex.Message);
            stringBuilder.AppendLine("Stack Trace: " + ex.StackTrace);
            stringBuilder.AppendLine();
            stringBuilder.Append(ex.ParseInnerExceptions("   "));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Determines whether [is file being used by another process].
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static bool IsFileBeingUsedByAnotherProcess(this IOException exception)
        {
            int num = Marshal.GetHRForException(exception) & ushort.MaxValue;
            if (num != 32)
                return num == 33;
            return true;
        }
    }
}
