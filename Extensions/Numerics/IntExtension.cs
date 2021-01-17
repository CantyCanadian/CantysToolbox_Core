using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class IntExtension
    {
        #region Bit Operations

        /// <summary>
        /// Checks if the bits contains the given value.
        /// </summary>
        public static bool CheckIfBitsContains(this int value, int contains)
        {
            return (value & contains) == contains;
        }

        /// <summary>
        /// Gets the value of a bit at a specific position inside an int as a bool.
        /// </summary>
        public static bool GetBitAtPosition(this int value, int position)
        {
            return ((value >> position) & 1) == 1;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 1.
        /// </summary>
        public static void SetBitAtPosition(this int value, int position)
        {
            value |= 1 << position;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 0.
        /// </summary>
        public static void ClearBitAtPosition(this int value, int position)
        {
            value &= ~(1 << position);
        }

        /// <summary>
        /// Flips the value of a bit at a specific position inside an int.
        /// </summary>
        public static void ToggleBitAtPosition(this int value, int position)
        {
            value ^= 1 << position;
        }

        #endregion

        /// <summary>
        /// Int version of Mathf.Sign.
        /// </summary>
        public static int Sign(this int target)
        {
            return (target > 0 ? 1 : 0) - (target < 0 ? 1 : 0);
        }

        /// <summary>
        /// Int version of Mathf.Clamp.
        /// </summary>
        public static int Clamp(this int target, int min, int max, bool overwrite = true)
        {
            int result = Mathf.Clamp(target, min, max);
            if (overwrite)
            {
                target = result;
            }
            return target;
        }

        /// <summary>
        /// Inverse of Clamp. If the value is between min and max, it gets pushed out to the nearest of the two.
        /// </summary>
        public static int InvClamp(this int target, int min, int max, bool overwrite = true)
        {
            int result = target;
            if (target > min && target < max)
            {
                float half = (float)max - (float)min;
                if (result < min)
                {
                    result = min;
                }
                else
                {
                    result = max;
                }
            }
            else
            {
                return target;
            }

            if (overwrite)
            {
                target = result;
            }
            return result;
        }
    }
}