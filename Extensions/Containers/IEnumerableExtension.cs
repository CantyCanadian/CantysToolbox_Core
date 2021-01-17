using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Base class for all containers. Only supports iteration.
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Applies an action onto each value in the container.
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null && action != null)
            {
                foreach(T element in source)
                {
                    action.Invoke(element);
                }
            }
            return source;
        }

        /// <summary>
        /// Returns a sub-container with the values starting from the start index.
        /// </summary>
        public static IEnumerable<T> Subdivide<T>(this IEnumerable<T> source, int start)
        {
            List<T> result = new List<T>();
            if (source != null && start >= 0)
            {
                int index = 0;
                foreach (T element in source)
                {
                    if (index >= start)
                    {
                        result.Add(element);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a sub-container with the values starting from the start index.
        /// </summary>
        public static IEnumerable<T> Subdivide<T>(this IEnumerable<T> source, int start, int count)
        {
            List<T> result = new List<T>();
            if (source != null && start >= 0)
            {
                int index = 0;
                foreach(T element in source)
                {
                    if (element != null && index >= start && index <= start + count)
                    {
                        result.Add(element);
                    }
                    index++;
                }
                return result;
            }
            return source;
        }

        /// <summary>
        /// Extract all the unique values from the container.
        /// </summary>
        public static IEnumerable<T> ExtractUniques<T>(this IEnumerable<T> source)
        {
            List<T> result = new List<T>();

            foreach (T item in source)
            {
                if (!result.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Extract all the duplicates from the container.
        /// </summary>
        public static IEnumerable<T> ExtractDuplicates<T>(this IEnumerable<T> source)
        {
            Dictionary<T, bool> dupes = new Dictionary<T, bool>();

            foreach (T item in source)
            {
                if (dupes.ContainsKey(item))
                {
                    dupes[item] = true;
                }
                else
                {
                    dupes.Add(item, false);
                }
            }

            List<T> result = new List<T>();

            foreach (KeyValuePair<T, bool> kv in dupes)
            {
                if (kv.Value)
                {
                    result.Add(kv.Key);
                }
            }

            return result;
        }

        /// <summary>
        /// Extract all the duplicates from the container, but only if they are duplicated X amount of times.
        /// </summary>
        public static IEnumerable<T> ExtractDuplicates<T>(this IEnumerable<T> source, int dupeCount)
        {
            Dictionary<T, int> dupes = new Dictionary<T, int>();

            foreach (T i in source)
            {
                if (dupes.ContainsKey(i))
                {
                    dupes[i]++;
                }
                else
                {
                    dupes.Add(i, 1);
                }
            }

            List<T> result = new List<T>();

            foreach (KeyValuePair<T, int> kv in dupes)
            {
                if (kv.Value >= dupeCount)
                {
                    result.Add(kv.Key);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a debug string using the values in the container.
        /// </summary>
        public static string ToDebugString<T>(this IEnumerable<T> source)
        {
            string result = "";

            foreach (T item in source)
            {
                result += $"[{item.ToString()}], ";
            }

            result = result.Remove(result.Length - 3, 2);

            return result;
        }
    }
}