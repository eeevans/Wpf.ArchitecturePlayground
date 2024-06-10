using System;
using System.Data;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>Gets the nullable.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <returns></returns>
        public static T? GetNullable<T>(this IDataReader reader, int fieldIndex) where T : struct
        {
            object obj = reader.GetValue(fieldIndex);
            if (!(obj is DBNull))
                return new T?((T)obj);
            return new T?();
        }

        /// <summary>Safes the get.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static T SafeGet<T>(this IDataReader reader, string column)
        {
            object obj = reader[column];
            if (obj != DBNull.Value)
                return (T)obj;
            return default;
        }

        /// <summary>Safes the get.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldIndex">Index of the field.</param>
        /// <returns></returns>
        public static T SafeGet<T>(this IDataReader reader, int fieldIndex)
        {
            object obj = reader[fieldIndex];
            if (obj != DBNull.Value)
                return (T)obj;
            return default;
        }
    }
}
