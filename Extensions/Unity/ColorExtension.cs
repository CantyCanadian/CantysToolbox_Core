using UnityEngine;

namespace Canty
{
    public static class ColorExtension
    {
        private const float m_ColorOffset = 0.0625f;

        /// <summary>
        /// Lightens the original color by a small offset (6.25%).
        /// </summary>
        /// <param name="overwrite">If we want to overwrite the original source with the result.</param>
        public static Color Lighter(this Color color, bool overwrite = false)
        {
            return color.BrightnessOffset(m_ColorOffset, overwrite);
        }

        /// <summary>
        /// Darkens the original color by a small offset (6.25%).
        /// </summary>
        /// <param name="overwrite">If we want to overwrite the original source with the result.</param>
        public static Color Darker(this Color color, bool overwrite = false)
        {
            return color.BrightnessOffset(-m_ColorOffset, overwrite);
        }

        /// <summary>
        /// Change the original color by a given offset.
        /// </summary>
        /// <param name="overwrite">If we want to overwrite the original source with the result.</param>
        public static Color BrightnessOffset(this Color color, float offset, bool overwrite = false)
        {
            Color result = new Color(color.r + offset, color.g + offset, color.b + offset, color.a);
            if (overwrite)
            {
                color = result;
            }
            return result;
        }

        /// <summary>
        /// Converts color to color32.
        /// </summary>
        public static Color32 ToColor32(this Color color)
        {
            return new Color32((byte)(color.r * 255.0f), (byte)(color.g * 255.0f), (byte)(color.b * 255.0f), (byte)(color.a * 255.0f));
        }

        /// <summary>
        /// Returns a hexadecimal string version of the given color.
        /// </summary>
        public static string ToHex(this Color color, bool withAlpha = true)
        {
            string result = string.Format("#{0:X2}{1:X2}{2:X2}", (int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
            if (withAlpha)
            {
                result += string.Format("{0:X2}", (int)(color.a * 255));
            }
            return result;
        }
    }
}