using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPlayground.Core.Extensions;

namespace WpfPlayground.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>To the set.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static ISet<T> ToSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }

        /// <summary>To the delimited string.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="formatArgsFunc">The format arguments function.</param>
        /// <returns></returns>
        public static string ToDelimitedString<T>(
          this IEnumerable<T> items,
          string delimiter,
          string formula = null,
          Func<T, object[]> formatArgsFunc = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = true;
            foreach (T obj in items)
            {
                if (!flag)
                    stringBuilder.Append(delimiter);
                flag = false;
                string format = formula ?? "{0}";
                object[] objArray1;
                if (formatArgsFunc != null)
                    objArray1 = formatArgsFunc(obj);
                else
                    objArray1 = new object[1] { obj };
                object[] objArray2 = objArray1;
                stringBuilder.AppendFormat(format, objArray2);
            }
            return stringBuilder.ToString();
        }

        /// <summary>Determines whether this instance is empty.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            return !items.Any();
        }

        /// <summary>Fors the each.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items.IsNull())
                return;
            foreach (T obj in items)
                action(obj);
        }

        /// <summary>Concats the specified object.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T obj)
        {
            return enumerable.Concat(new T[1]
            {
        obj
            });
        }

        /// <summary>Prepends the specified object.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static IEnumerable<T1> Prepend<T1, T2>(
          this IEnumerable<T1> collection,
          T2 obj)
          where T2 : T1
        {
            return (new T1[1]
            {
         obj
            }).Union(collection);
        }

        /// <summary>Appends the specified object.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static IEnumerable<T1> Append<T1, T2>(this IEnumerable<T1> collection, T2 obj) where T2 : T1
        {
            return collection.Union((new T1[1]
            {
         obj
            }).AsEnumerable());
        }

        /// <summary>Appends the specified new items.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="newItems">The new items.</param>
        /// <returns></returns>
        public static IEnumerable<T> Append<T>(
          this IEnumerable<T> items,
          IEnumerable<T> newItems)
        {
            return items.Union(newItems);
        }

        /// <summary>Fors the each.</summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<K, V>(
          this IEnumerable<KeyValuePair<K, V>> dictionary,
          Action<K, V> action)
        {
            foreach (KeyValuePair<K, V> keyValuePair in dictionary)
                action(keyValuePair.Key, keyValuePair.Value);
        }

        /// <summary>
        /// Get the next value off an enumerator if it exists or the default if there is no next
        /// </summary>
        public static T GetNext<T>(this IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext())
                return default;
            return enumerator.Current;
        }

        /// <summary>
        /// Generate an infinite sequence. Again, this is an INFINITE sequence. Do not run any method that iterates through it
        /// to the end.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="generator"></param>
        /// <returns></returns>
        public static IEnumerable<T> Sequence<T>(Func<int, T> generator)
        {
            int i = 0;
            while (true)
                yield return generator(++i);
        }

        /// <summary>Flattens the hierarchy.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node.</param>
        /// <param name="getChildEnumerator">The get child enumerator.</param>
        /// <returns></returns>
        public static IEnumerable<T> FlattenHierarchy<T>(
          this T node,
          Func<T, IEnumerable<T>> getChildEnumerator)
        {
            yield return node;
            if (getChildEnumerator(node) != null)
            {
                foreach (T obj in getChildEnumerator(node).SelectMany(child => child.FlattenHierarchy(getChildEnumerator)))
                    yield return obj;
            }
        }

        /// <summary>Generates the hashcode.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="nullValueHash">The null value hash.</param>
        /// <returns></returns>
        public static int GenerateHashcode<T>(
          this IEnumerable<T> enumerable,
          int seed,
          int nullValueHash)
        {
            int num = seed;
            foreach (T obj in enumerable)
                num ^= obj == null ? nullValueHash : obj.GetHashCode();
            return num;
        }

        /// <summary>Fors the each in background.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="errorCallback">The error callback.</param>
        /// <param name="batchSize">Size of the batch.</param>
        public static void ForEachInBackground<T>(
          this IEnumerable<T> items,
          Action<T> action,
          Action callback,
          Action<Exception> errorCallback,
          int batchSize)
        {
            Task.Factory.StartNew(_ => ForEachInBackground(new Queue<T>(items), action, callback, errorCallback, batchSize), null, TaskCreationOptions.AttachedToParent);
        }

        /// <summary>Batches the specified batch size.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="action">The action.</param>
        public static void Batch<T>(
          this IEnumerable<T> items,
          int batchSize,
          Action<IEnumerable<T>, ulong> action)
        {
            ulong num = 0;
            List<T> source = new List<T>();
            foreach (T obj in items)
            {
                source.Add(obj);
                if (source.Count == batchSize)
                {
                    action(source, num);
                    ++num;
                    source.Clear();
                }
            }
            if (source.Any())
                action(source, num);
            source.Clear();
        }

        /// <summary>Batches the specified batch size.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="action">The action.</param>
        public static void Batch<T>(
          this IEnumerable<T> items,
          int batchSize,
          Action<IEnumerable<T>> action)
        {
            items.Batch(batchSize, (batch, batchNumber) => action(batch));
        }

        /// <summary>Fors the each in background.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="action">The action.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="errorCallback">The error callback.</param>
        /// <param name="batchSize">Size of the batch.</param>
        private static void ForEachInBackground<T>(
          Queue<T> queue,
          Action<T> action,
          Action callback,
          Action<Exception> errorCallback,
          int batchSize)
        {
            try
            {
                if (queue.Count == 0)
                {
                    if (callback == null)
                        return;
                    callback();
                }
                else
                {
                    for (int index = 0; queue.Count > 0 && (batchSize <= 0 || index < batchSize); ++index)
                        action(queue.Dequeue());
                    Task.Factory.StartNew(_ => ForEachInBackground(queue, action, callback, errorCallback, batchSize), null, TaskCreationOptions.AttachedToParent);
                }
            }
            catch (Exception ex)
            {
                if (errorCallback == null)
                    return;
                errorCallback(ex);
            }
        }

        /// <summary>Commas the separate.</summary>
        /// <param name="strings">The strings.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static string CommaSeparate(
          this IEnumerable<string> strings,
          Func<string, string> selector)
        {
            return strings.Select(selector).Join(", ");
        }
    }
}
