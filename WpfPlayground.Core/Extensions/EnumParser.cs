using System;
using WpfPlayground.Core.Exceptions;

namespace WpfPlayground.Core.Extensions
{
    public static class EnumParser
    {
        public static T Parse<T>(string o) where T : struct
        {
            T result;
            if (Enum.TryParse(o, true, out result)) return result;

            throw new WpfPlaygroundException("Invalid {0} value: {1}", typeof(T).Name, o);
        }
    }
}