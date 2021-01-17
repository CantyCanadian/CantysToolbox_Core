using System.Linq;
using System.Text.RegularExpressions;

namespace Canty
{
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a string using a Regex pattern.
        /// </summary>
        /// <param name="pattern">Regex string to apply on the source string.</param>
        /// <returns>Array containing the resulting Regex matches.</returns>
        public static string[] SplitUsingRegex(this string source, string pattern)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new string[] { source };
            }

            return Regex.Matches(source, pattern).OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
        }

        /// <summary>
        /// Splits a string by its capital letters.
        /// </summary>
        /// <returns>Array containing each words.</returns>
        public static string[] SplitCamelCase(this string source)
        {
            return SplitUsingRegex(source, @"[A-Z][a-z]*|[a-z]+|\d+");
        }

        /// <summary>
        /// Adds a space between each capital letters.
        /// </summary>
        /// <param name="overwrite">If we want to overwrite the original source with the result.</param>
        /// <returns>Source string with a space between each words.</returns>
        public static string SpaceCamelCase(this string source, bool overwrite = false)
        {
            string[] values = SplitUsingRegex(source, @"[A-Z][a-z]*|[a-z]+|\d+");
            string result = "";
            foreach (string s in values)
            {
                result += s + " ";
            }
            result.Remove(result.Length - 1);

            if (overwrite)
            {
                source = result;
            }
            return result;
        }

        /// <summary>
        /// Limits the size of the string, adding a mark at the end (default is ...)
        /// </summary>
        /// <param name="overwrite">If we want to overwrite the original source with the result.</param>
        /// <returns>Truncated source.</returns>
        public static string Truncate(this string source, int maxChars, bool overwrite = false, string truncationMark = "...")
        {
            string result = source.Length <= maxChars ? source : source.Substring(0, maxChars) + truncationMark;
            if (overwrite)
            {
                source = result;
            }
            return source;
        }
    }
}