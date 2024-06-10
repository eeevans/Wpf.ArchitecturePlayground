using System;
using System.Collections.Generic;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>Adds to.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="addTo">The add to.</param>
        /// <returns></returns>
        public static T AddTo<T>(this T item, IList<T> addTo)
        {
            addTo.Add(item);
            return item;
        }

        /// <summary>Adds to.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="addTo">The add to.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TValue AddTo<TKey, TValue>(
          this IDictionary<TKey, TValue> addTo,
          TKey key,
          TValue value)
        {
            addTo.Add(key, value);
            return value;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the item in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static int? IndexOf<T>(this IEnumerable<T> list, T item)
        {
            return list.IndexOf(x => x.Equals(item));
        }

        /// <summary>
        /// Returns the index of the first item matched by the predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int? IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            int num = 0;
            foreach (T obj in enumerable)
            {
                if (predicate(obj))
                    return new int?(num);
                ++num;
            }
            return new int?();
        }
    }
}
