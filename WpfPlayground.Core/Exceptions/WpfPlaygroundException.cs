using System;
using System.Runtime.Serialization;

namespace WpfPlayground.Core.Exceptions
{
    internal class WpfPlaygroundException : Exception
    {
        public WpfPlaygroundException()
           : base() { }

        public WpfPlaygroundException(string message, params object[] args)
            : base(string.Format(message, args)) { }

        public WpfPlaygroundException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException) { }

        public WpfPlaygroundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
