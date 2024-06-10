using System.Collections;
using System.Collections.Generic;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>Adds the sorted.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        public static void AddSorted<T>(this IList<T> list, T item)
        {
            list.Add(item);
            ArrayList.Adapter((IList)list).Sort();
        }
    }
}
