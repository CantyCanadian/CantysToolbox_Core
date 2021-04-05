using Canty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Canty
{
    public static class LongExtension
    {
        #region Bit Operations

        /// <summary>
        /// Checks if the bits contains the given value.
        /// </summary>
        public static bool CheckIfBitsContains(this long value, long contains)
        {
            return (value & contains) == contains;
        }

        /// <summary>
        /// Gets the value of a bit at a specific position inside an int as a bool.
        /// </summary>
        public static bool GetBitAtPosition(this long value, int position)
        {
            return ((value >> position) & 1L) == 1L;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 1.
        /// </summary>
        public static void SetBitAtPosition(this long value, int position)
        {
            value |= 1L << position;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 0.
        /// </summary>
        public static void ClearBitAtPosition(this long value, int position)
        {
            value &= ~(1L << position);
        }

        /// <summary>
        /// Flips the value of a bit at a specific position inside an int.
        /// </summary>
        public static void ToggleBitAtPosition(this long value, int position)
        {
            value ^= 1L << position;
        }

        #endregion

        /// <summary>
        /// Long version of Mathf.Sign.
        /// </summary>
        public static int Sign(this long target)
        {
            return (target > 0 ? 1 : 0) - (target < 0 ? 1 : 0);
        }

        /// <summary>
        /// Long version of Mathf.Clamp.
        /// </summary>
        public static long Clamp(this long target, long min, long max, bool overwrite = true)
        {
            long result = target > max ? max : (target < min ? min : target);
            if (overwrite)
            {
                target = result;
            }
            return target;
        }

        /// <summary>
        /// Inverse of Clamp. If the value is between min and max, it gets pushed out to the nearest of the two.
        /// </summary>
        public static long InvClamp(this long target, long min, long max, bool overwrite = true)
        {
            long result = target;
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