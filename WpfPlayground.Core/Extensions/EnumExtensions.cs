using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>Gets this instance.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Get<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>Gets the flags.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetFlags<T>(Enum input)
        {
            foreach (object obj in Enum.GetValues(input.GetType()))
            {
                if (IsPowerOfTwo(Convert.ToUInt64(obj)) && input.HasFlag((Enum)obj))
                    yield return (T)obj;
            }
        }

        private static bool IsPowerOfTwo(ulong x)
        {
            if (x == 0UL)
                return false;
            long num = (long)x;
            return (num & num - 1L) == 0L;
        }
    }
}
