using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Inherits from ICollection. Supports iteration and in/out as well as indexing.
    /// </summary>
    public static class IListExtension
    {
        /// <summary>
        /// Remove a range of value from the container.
        /// </summary>
        public static IList<T> RemoveRange<T>(this IList<T> source, int start, int count)
        {
            if (source != null && start >= 0 && start + count <= source.Count)
            {
                for(int i = start; i <= start + count; i++)
                {
                    source.RemoveAt(i);
                }
            }
            return source;
        }
    }
}