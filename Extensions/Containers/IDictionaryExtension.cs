using System;
using System.Collections.Generic;

namespace Canty
{
    public static class IDictionaryExtension
    {
        /// <summary>
        /// Append the values from another dictionary into the source dictionary.
        /// </summary>
        public static Dictionary<K, V> Append<K, V>(this Dictionary<K, V> source, Dictionary<K, V> target)
        {
            if (source != null && target != null)
            {
                foreach (KeyValuePair<K, V> item in source)
                {
                    if (!target.ContainsKey(item.Key))
                    {
                        target.Add(item.Key, item.Value);
                    }
                }
            }
            return source;
        }

        /// <summary>
        /// Returns a dictionary with the values converted to another type using a function.
        /// </summary>
        public static Dictionary<K, V2> ConvertValues<K, V1, V2>(this Dictionary<K, V1> source, Func<V1, V2> function)
        {
            Dictionary<K, V2> result = new Dictionary<K, V2>();
            foreach(KeyValuePair<K, V1> kvp in source)
            {
                result.Add(kvp.Key, function.Invoke(kvp.Value));
            }
            return result;
        }
    }
}