using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>Ifs the not null then.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="action">The action.</param>
        public static void IfNotNullThen<T>(this T o, Action<T> action) where T : class
        {
            if (!o.IsNotNull())
                return;
            action(o);
        }

        /// <summary>
        /// Determines whether [is in list] [the specified arguments].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static bool IsInList<T>(this T o, params T[] args)
        {
            return args.Any(x => o.Equals(x));
        }

        /// <summary>Ifs the not null.</summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="returnFunc">The return function.</param>
        /// <param name="elseVal">The else value.</param>
        /// <returns></returns>
        public static R IfNotNull<T, R>(this T o, Func<T, R> returnFunc, R elseVal)
        {
            if (o.IsNull())
                return elseVal;
            return returnFunc(o);
        }

        /// <summary>Ifs the not null.</summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="returnFunc">The return function.</param>
        /// <returns></returns>
        public static R IfNotNull<T, R>(this T o, Func<T, R> returnFunc)
        {
            return o.IfNotNull(returnFunc, default);
        }

        /// <summary>Ifs the type of the is of.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="action">The action.</param>
        public static void IfIsOfType<T>(this object o, Action<T> action) where T : class
        {
            T obj = o as T;
            if (obj == null)
                return;
            action(obj);
        }

        /// <summary>Ifs the type of the is of.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static bool IfIsOfType<T>(this object o, Func<T, bool> predicate) where T : class
        {
            T obj = o as T;
            if (obj == null)
                return false;
            return predicate(obj);
        }

        /// <summary>Determines whether [is type of].</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static bool IsTypeOf<T>(this object o)
        {
            return o.IsTypeOf(typeof(T));
        }

        /// <summary>Determines whether [is type of] [the specified type].</summary>
        /// <param name="o">The o.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsTypeOf(this object o, Type type)
        {
            return o.GetType() == type;
        }

        /// <summary>Ases the specified o.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static T As<T>(this object o) where T : class
        {
            return o as T;
        }

        /// <summary>Determines whether this instance is null.</summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>Determines whether [is not null].</summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>Determines whether this instance is true.</summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsTrue(this object obj)
        {
            if (obj.IsNotNull())
                return (bool)obj;
            return false;
        }

        /// <summary>Tries the get value.</summary>
        /// <typeparam name="KEY">The type of the ey.</typeparam>
        /// <typeparam name="VALUE">The type of the alue.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static VALUE TryGetValue<KEY, VALUE>(this IDictionary<KEY, VALUE> dict, KEY key)
        {
            if (!dict.ContainsKey(key))
                return default;
            return dict[key];
        }

        /// <summary>Forces the set value.</summary>
        /// <typeparam name="KEY">The type of the ey.</typeparam>
        /// <typeparam name="VALUE">The type of the alue.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="prevToNewValue">The previous to new value.</param>
        /// <param name="default">The default.</param>
        public static void ForceSetValue<KEY, VALUE>(
          this IDictionary<KEY, VALUE> dict,
          KEY key,
          Func<VALUE, VALUE> prevToNewValue,
          VALUE @default = null) where VALUE : class
        {
            if (dict.ContainsKey(key))
                dict[key] = prevToNewValue(dict[key]);
            else
                dict.Add(key, prevToNewValue(@default));
        }

        /// <summary>Wraps the in enumerable.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static IEnumerable<T> WrapInEnumerable<T>(this T obj)
        {
            if (obj.IsNull())
                return new T[0];
            return new T[1] { obj };
        }

        /// <summary>Nulls the safe equals.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool NullSafeEquals(this object x, object y)
        {
            if (x.IsNull() && y.IsNull())
                return true;
            if (x.IsNotNull() && y.IsNotNull())
                return x.Equals(y);
            return false;
        }

        /// <summary>Converts the type of to nullable.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object ConvertToNullableType(this object value)
        {
            if (value == null)
                return value;
            Type type = value.GetType();
            if ((!type.IsGenericType ? 0 : type.GetGenericTypeDefinition() == typeof(Nullable<>) ? 1 : 0) != 0 || !type.IsValueType)
                return value;
            return new NullableConverter(typeof(Nullable<>).MakeGenericType(type)).ConvertFrom(value);
        }
    }
}
