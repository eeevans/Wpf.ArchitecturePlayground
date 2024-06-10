using System;
using System.Text;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>Exceptions the specified ex.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="ex">The ex.</param>
        public static void Exception(this ILog logger, Exception ex)
        {
            logger.Error((object)ex.GetStackTrace());
        }

        /// <summary>Gets the stack trace.</summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string GetStackTrace(this Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Exception");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Exception Message: " + ex.Message);
            stringBuilder.AppendLine("Stack Trace: " + ex.StackTrace);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Inner Exception:");
                stringBuilder.AppendLine("Exception Message: " + ex.Message);
                stringBuilder.AppendLine("Stack Trace: " + ex.StackTrace);
            }
            return stringBuilder.ToString();
        }
    }

    public interface ILog
    {
        void Error(object stackTrace);
    }
}
