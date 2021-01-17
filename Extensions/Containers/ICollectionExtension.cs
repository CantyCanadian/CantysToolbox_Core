using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Inherits from IEnumerable. Supports iteration as well as in/out.
    /// </summary>
    public static class ICollectionExtension
    {
        /// <summary>
        /// Appends another collection to the source collection.
        /// </summary>
        public static ICollection<T> Append<T>(this ICollection<T> source, ICollection<T> items)
        {
            if (source != null && items != null)
            {
                foreach (T item in items)
                {
                    if (item != null)
                    {
                        source.Add(item);
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Adds multiple items to the source collection.
        /// </summary>
        public static ICollection<T> AddMultiple<T>(this ICollection<T> source, params T[] items)
        {
            if (source != null && items != null)
            {
                foreach (T item in items)
                {
                    if (item != null)
                    {
                        source.Add(item);
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Adds an item but only if the item isn't already in the collection.
        /// </summary>
        public static ICollection<T> AddOnce<T>(this ICollection<T> source, T item)
        {
            if (source != null && item != null)
            {
                if (!source.Contains(item))
                {
                    source.Add(item);
                }
            }

            return source;
        }

        /// <summary>
        /// Appends another collection to the source collection, but each item is checked to make sure no duplicate is present.
        /// </summary>
        public static ICollection<T> AppendOnce<T>(this ICollection<T> source, ICollection<T> items)
        {
            if (source != null && items != null)
            {
                foreach (T item in items)
                {
                    if (item != null && !source.Contains(item))
                    {
                        source.Add(item);
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Adds multiple items to the source collection, but each item is checked to make sure no duplicate is present.
        /// </summary>
        public static ICollection<T> AddMultipleOnce<T>(this ICollection<T> source, params T[] items)
        {
            if (source != null && items != null)
            {
                foreach (T item in items)
                {
                    if (item != null && !source.Contains(item))
                    {
                        source.Add(item);
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        public static T GetRandom<T>(this ICollection<T> source)
        {
            if (source != null)
            {
                int random = Random.Range(0, source.Count);
                int index = 0;
                foreach (T item in source)
                {
                    if (index == random)
                    {
                        return item;
                    }
                    index++;
                }
            }
            return default;
        }

        /// <summary>
        /// Removes the last item from the collection and returns it.
        /// </summary>
        public static T Pop<T>(this ICollection<T> source)
        {
            if (source != null && source.Count > 0)
            {
                T poppedItem = default;
                foreach (T item in source)
                {
                    poppedItem = item;
                }
                source.Remove(poppedItem);
                return poppedItem;
            }
            return default;
        }

        /// <summary>
        /// Returns the last value from the collection (latest added).
        /// </summary>
        public static T Last<T>(this ICollection<T> source)
        {
            if (source != null && source.Count > 0)
            {
                T topItem = default;
                foreach (T item in source)
                {
                    topItem = item;
                }
                return topItem;
            }
            return default;
        }

        /// <summary>
        /// Returns the first value from the collection (first added).
        /// </summary>
        public static T First<T>(this ICollection<T> source)
        {
            if (source != null && source.Count > 0)
            {
                T bottomItem = default;
                foreach (T item in source)
                {
                    bottomItem = item;
                    break;
                }
                return bottomItem;
            }
            return default;
        }
    }
}