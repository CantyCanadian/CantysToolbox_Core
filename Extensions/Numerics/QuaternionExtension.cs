///====================================================================================================
///
///     QuaternionExtension by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty
{
    public static class QuaternionExtension
    {
        /// <summary>
        /// Directly converts Quaternion to Vector4.
        /// </summary>
        /// <returns>Vector4 made from Quaternion.</returns>
        public static Vector4 ToVector4(this Quaternion source)
        {
            return new Vector4(source.x, source.y, source.z, source.w);
        }
    }
}