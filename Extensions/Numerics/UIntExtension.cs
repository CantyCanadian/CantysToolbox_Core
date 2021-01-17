using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    public static class UIntExtension
    {
        #region Bit Operations

        /// <summary>
        /// Checks if the bits contains the given value.
        /// </summary>
        public static bool CheckIfBitsContains(ref uint value, int contains)
        {
            return (value & contains) == contains;
        }

        /// <summary>
        /// Gets the value of a bit at a specific position inside an int as a bool.
        /// </summary>
        public static bool GetBitAtPosition(ref uint value, int position)
        {
            return ((value >> position) & 1) == 1;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 1.
        /// </summary>
        public static void SetBitAtPosition(ref uint value, int position)
        {
            value |= 1u << position;
        }

        /// <summary>
        /// Sets the value of a bit at a specific position inside an int to 0.
        /// </summary>
        public static void ClearBitAtPosition(ref uint value, int position)
        {
            value &= ~(1u << position);
        }

        /// <summary>
        /// Flips the value of a bit at a specific position inside an int.
        /// </summary>
        public static void ToggleBitAtPosition(ref uint value, int position)
        {
            value ^= 1u << position;
        }

        #endregion

        /// <summary>
        /// Int version of Mathf.Clamp.
        /// </summary>
        public static uint Clamp(this uint target, uint min, uint max, bool overwrite = true)
        {
            uint result = (uint)Mathf.Clamp(target, min, max);
            if (overwrite)
            {
                target = result;
            }
            return target;
        }

        /// <summary>
        /// Inverse of Clamp. If the value is between min and max, it gets pushed out to the nearest of the two.
        /// </summary>
        public static uint InvClamp(this uint target, uint min, uint max, bool overwrite = true)
        {
            uint result = target;
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