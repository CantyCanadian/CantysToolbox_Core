using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class ColorUtil
    {
        /// <summary>
        /// Returns a random bright color (rgb between 0.4f and 1.0f).
        /// </summary>
        public static Color RandomBright()
        {
            return RandomBright(1.0f);
        }

        /// <summary>
        /// Returns a random bright color (rgb between 0.4f and 1.0f) with alpha of given value.
        /// </summary>
        public static Color RandomBright(float alpha)
        {
            Mathf.Clamp01(alpha);
            return new Color(Random.Range(.4f, 1), Random.Range(.4f, 1), Random.Range(.4f, 1), alpha);
        }

        /// <summary>
        /// Returns a random dim color (rgb between 0.2f and 0.6f).
        /// </summary>
        public static Color RandomDim()
        {
            return RandomDim(1.0f);
        }

        /// <summary>
        /// Returns a random dim color (rgb between 0.2f and 0.6f) with alpha of given value.
        /// </summary>
        public static Color RandomDim(float alpha)
        {
            Mathf.Clamp01(alpha);
            return new Color(Random.Range(.2f, .6f), Random.Range(.2f, .8f), Random.Range(.2f, .8f), alpha);
        }

        /// <summary>
        /// Returns a random color with max alpha.
        /// </summary>
        public static Color RandomColor()
        {
            return RandomColor(0.0f, 1.0f);
        }

        /// <summary>
        /// Returns a random color between the given values with max alpha.
        /// </summary>
        public static Color RandomColor(float min, float max)
        {
            Mathf.Clamp01(min);
            Mathf.Clamp01(max);
            return new Color(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }

        /// <summary>
        /// Returns a random color with the given alpha.
        /// </summary>
        public static Color RandomColor(float alpha)
        {
            return RandomColor(0.0f, 1.0f, alpha);
        }

        /// <summary>
        /// Returns a random color between the given values with the given alpha.
        /// </summary>
        public static Color RandomColor(float min, float max, float alpha)
        {
            Mathf.Clamp01(min);
            Mathf.Clamp01(max);
            return new Color(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max), alpha);
        }
    }
}