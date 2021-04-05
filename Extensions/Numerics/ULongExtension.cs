using Canty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Canty
{
    public static class ULongExtension
    {
        #region Bit Operations

        /// <summary>
        /// Checks if the bits contains the given value.
        /// </summary>
        public static bool CheckIfBitsContains(this ulong value, ulong contains)
        {
            return (value & contains) == contains;
        }

        /// <summary>
        /// Gets the value of a bit at a specific position inside an int as a bool.
        /// </summary>
        public static bool GetBitAtPosition(this ulong value, int position)
        {
            return ((value >> position) & 1L) == 1L;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 1.
        /// </summary>
        public static void SetBitAtPosition(this ulong value, int position)
        {
            value |= 1UL << position;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 0.
        /// </summary>
        public static void ClearBitAtPosition(this ulong value, int position)
        {
            value &= ~(1UL << position);
        }

        /// <summary>
        /// Flips the value of a bit at a specific position inside an int.
        /// </summary>
        public static void ToggleBitAtPosition(this ulong value, int position)
        {
            value ^= 1UL << position;
        }

        #endregion

        /// <summary>
        /// Long version of Mathf.Clamp.
        /// </summary>
        public static ulong Clamp(this ulong target, ulong min, ulong max, bool overwrite = true)
        {
            ulong result = target > max ? max : (target < min ? min : target);
            if (overwrite)
            {
                target = result;
            }
            return target;
        }

        /// <summary>
        /// Inverse of Clamp. If the value is between min and max, it gets pushed out to the nearest of the two.
        /// </summary>
        public static ulong InvClamp(this ulong target, ulong min, ulong max, bool overwrite = true)
        {
            ulong result = target;
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