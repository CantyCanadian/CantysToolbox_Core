using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class FloatExtension
    {
        /// <summary>
        /// Simple wrapper for Mathf.Sign.
        /// </summary>
        public static float Sign(this float target)
        {
            return Mathf.Sign(target);
        }

        /// <summary>
        /// Simple wrapper for Mathf.Clamp.
        /// </summary>
        public static float Clamp(this float target, float min, float max, bool overwrite = true)
        {
            float result = Mathf.Clamp(target, min, max);
            if (overwrite)
            {
                target = result;
            }
            return result;
        }

        /// <summary>
        /// Inverse of Clamp. If the value is between min and max, it gets pushed out to the nearest of the two.
        /// </summary>
        public static float InvClamp(this float target, float min, float max, bool overwrite = true)
        {
            if (target > min && target < max)
            {
                float result = target;
                float half = max - min;
                if (result < min)
                {
                    result = min;
                }
                else
                {
                    result = max;
                }

                if (overwrite)
                {
                    target = result;
                }
                return result;
            }
            else
            {
                return target;
            }
        }

        /// <summary>
        /// Returns the closest float to target.
        /// </summary>
        public static float ClosestPoint(this float target, params float[] points)
        {
            float closest = float.MaxValue;
            float dist = float.MaxValue;
            foreach(float point in points)
            {
                float diff = Mathf.Abs(target - point);
                if (diff < dist)
                {
                    dist = diff;
                    closest = point;
                }
            }
            return closest;
        }

        /// <summary>
        /// Returns wether or not the closest float to target is the value of pointA compared to pointB.
        /// </summary>
        public static bool IsClosestPointA(this float target, float pointA, float pointB)
        {
            return Mathf.Abs(target - pointA) <= Mathf.Abs(target - pointB);
        }
    }
}