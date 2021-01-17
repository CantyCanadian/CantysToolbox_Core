using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Canty
{
    public static class Texture2DExtension
    {
        /// <summary>
        /// Sets every value of a Texture2D to a specified color.
        /// </summary>
        public static Texture2D Fill(this Texture2D source, Color color)
        {
            return Fill(source, color.ToColor32());
        }

        /// <summary>
        /// Sets every value of a Texture2D to a specified color.
        /// </summary>
        public static Texture2D Fill(this Texture2D source, Color32 color)
        {
            Color32[] colorArray = new Color32[source.width * source.height];
            for (int i = 0; i < colorArray.Length; i++)
            {
                colorArray[i] = color;
            }

            source.SetPixels32(colorArray);
            source.Apply();
            return source;
        }
    }
}