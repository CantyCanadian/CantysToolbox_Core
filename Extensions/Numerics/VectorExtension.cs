///====================================================================================================
///
///     VectorExtension by
///     - CantyCanadian
///
///====================================================================================================

using UnityEngine;

namespace Canty
{
    public static class VectorExtention
    {
        #region AddScalar

        /// <summary>
        /// Simple vector function to add a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to add.</param>
        /// <returns>The result vector.</returns>
        public static Vector2 AddScalar(this Vector2 target, params float[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x += scalars[i];
                target.y += scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to add a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to add.</param>
        /// <returns>The result vector.</returns>
        public static Vector2Int AddScalar(this Vector2Int target, params int[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x += scalars[i];
                target.y += scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to add a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to add.</param>
        /// <returns>The result vector.</returns>
        public static Vector3 AddScalar(this Vector3 target, params float[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x += scalars[i];
                target.y += scalars[i];
                target.z += scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to add a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to add.</param>
        /// <returns>The result vector.</returns>
        public static Vector3Int AddScalar(this Vector3Int target, params int[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x += scalars[i];
                target.y += scalars[i];
                target.z += scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to add a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to add.</param>
        /// <returns>The result vector.</returns>
        public static Vector4 AddScalar(this Vector4 target, params float[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.w += scalars[i];
                target.x += scalars[1];
                target.y += scalars[i];
                target.z += scalars[i];
            }

            return target;
        }

        #endregion

        #region MinusScalar

        /// <summary>
        /// Simple vector function to minus a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to minus.</param>
        /// <returns>The result vector.</returns>
        public static Vector2 MinusScalar(this Vector2 target, params float[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x -= scalars[i];
                target.y -= scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to minus a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to minus.</param>
        /// <returns>The result vector.</returns>
        public static Vector2Int MinusScalar(this Vector2Int target, params int[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x -= scalars[i];
                target.y -= scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to minus a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to minus.</param>
        /// <returns>The result vector.</returns>
        public static Vector3 MinusScalar(this Vector3 target, params float[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x -= scalars[i];
                target.y -= scalars[i];
                target.z -= scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to minus a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to minus.</param>
        /// <returns>The result vector.</returns>
        public static Vector3Int MinusScalar(this Vector3Int target, params int[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.x -= scalars[i];
                target.y -= scalars[i];
                target.z -= scalars[i];
            }

            return target;
        }

        /// <summary>
        /// Simple vector function to minus a value to each values of the vector.
        /// </summary>
        /// <param name="scalars">Scalars to minus.</param>
        /// <returns>The result vector.</returns>
        public static Vector4 MinusScalar(this Vector4 target, params float[] scalars)
        {
            for (int i = 0; i < scalars.Length; i++)
            {
                target.w -= scalars[i];
                target.x -= scalars[1];
                target.y -= scalars[i];
                target.z -= scalars[i];
            }

            return target;
        }

        #endregion

        #region Multiply

        /// <summary>
        /// Simple vector multiplication function.
        /// </summary>
        /// <param name="vectors">Vectors to add.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector2 Multiply(this Vector2 target, params Vector2[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                target.x *= vectors[i].x;
                target.y *= vectors[i].y;
            }

            return target;
        }

        /// <summary>
        /// Simple vector multiplication function.
        /// </summary>
        /// <param name="vectors">Vectors to add.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector2Int Multiply(this Vector2Int target, params Vector2Int[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                target.x *= vectors[i].x;
                target.y *= vectors[i].y;
            }

            return target;
        }

        /// <summary>
        /// Simple vector multiplication function.
        /// </summary>
        /// <param name="vectors">Vectors to add.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector3 Multiply(this Vector3 target, params Vector3[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                target.x *= vectors[i].x;
                target.y *= vectors[i].y;
                target.z *= vectors[i].z;
            }

            return target;
        }

        /// <summary>
        /// Simple vector multiplication function.
        /// </summary>
        /// <param name="vectors">Vectors to add.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector3Int Multiply(this Vector3Int target, params Vector3Int[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                target.x *= vectors[i].x;
                target.y *= vectors[i].y;
                target.z *= vectors[i].z;
            }

            return target;
        }

        /// <summary>
        /// Simple vector multiplication function.
        /// </summary>
        /// <param name="vectors">Vectors to add.</param>
        /// <returns>The multiplied vector.</returns>
        public static Vector4 Multiply(this Vector4 target, params Vector4[] vectors)
        {
            for (int i = 0; i < vectors.Length; i++)
            {
                target.w *= vectors[i].w;
                target.x *= vectors[1].x;
                target.y *= vectors[i].y;
                target.z *= vectors[i].z;
            }

            return target;
        }

        #endregion

        #region Clamp

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2 Clamp(this Vector2 target, float min, float max)
        {
            target.x = Mathf.Clamp(target.x, min, max);
            target.y = Mathf.Clamp(target.y, min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2 Clamp(this Vector2 target, Vector2 min, Vector2 max)
        {
            target.x = Mathf.Clamp(target.x, min.x, max.x);
            target.y = Mathf.Clamp(target.y, min.y, max.y);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2Int Clamp(this Vector2Int target, int min, int max)
        {
            target.x = Mathf.Clamp(target.x, min, max);
            target.y = Mathf.Clamp(target.y, min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2Int Clamp(this Vector2Int target, Vector2Int min, Vector2Int max)
        {
            target.x = Mathf.Clamp(target.x, min.x, max.x);
            target.y = Mathf.Clamp(target.y, min.y, max.y);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3 Clamp(this Vector3 target, float min, float max)
        {
            target.x = Mathf.Clamp(target.x, min, max);
            target.y = Mathf.Clamp(target.y, min, max);
            target.z = Mathf.Clamp(target.z, min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3 Clamp(this Vector3 target, Vector3 min, Vector3 max)
        {
            target.x = Mathf.Clamp(target.x, min.x, max.x);
            target.y = Mathf.Clamp(target.y, min.y, max.y);
            target.z = Mathf.Clamp(target.z, min.z, max.z);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3Int Clamp(this Vector3Int target, int min, int max)
        {
            target.x = Mathf.Clamp(target.x, min, max);
            target.y = Mathf.Clamp(target.y, min, max);
            target.z = Mathf.Clamp(target.z, min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3Int Clamp(this Vector3Int target, Vector3Int min, Vector3Int max)
        {
            target.x = Mathf.Clamp(target.x, min.x, max.x);
            target.y = Mathf.Clamp(target.y, min.y, max.y);
            target.z = Mathf.Clamp(target.z, min.z, max.z);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector4 Clamp(this Vector4 target, float min, float max)
        {
            target.x = Mathf.Clamp(target.x, min, max);
            target.y = Mathf.Clamp(target.y, min, max);
            target.z = Mathf.Clamp(target.z, min, max);
            target.w = Mathf.Clamp(target.w, min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector4 Clamp(this Vector4 target, Vector4 min, Vector4 max)
        {
            target.x = Mathf.Clamp(target.x, min.x, max.x);
            target.y = Mathf.Clamp(target.y, min.y, max.y);
            target.z = Mathf.Clamp(target.z, min.z, max.z);
            target.w = Mathf.Clamp(target.w, min.w, max.w);

            return target;
        }

        #endregion

        #region Inverse Clamp

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2 InvClamp(this Vector2 target, float min, float max)
        {
            target.x = target.x.InvClamp(min, max);
            target.y = target.y.InvClamp(min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2 InvClamp(this Vector2 target, Vector2 min, Vector2 max)
        {
            target.x = target.x.InvClamp(min.x, max.y);
            target.y = target.y.InvClamp(min.x, max.y);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2Int InvClamp(this Vector2Int target, int min, int max)
        {
            target.x = target.x.InvClamp(min, max);
            target.y = target.y.InvClamp(min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector2Int InvClamp(this Vector2Int target, Vector2Int min, Vector2Int max)
        {
            target.x = target.x.InvClamp(min.x, max.y);
            target.y = target.y.InvClamp(min.x, max.y);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3 InvClamp(this Vector3 target, float min, float max)
        {
            target.x = target.x.InvClamp(min, max);
            target.y = target.y.InvClamp(min, max);
            target.z = target.y.InvClamp(min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3 InvClamp(this Vector3 target, Vector3 min, Vector3 max)
        {
            target.x = target.x.InvClamp(min.x, max.y);
            target.y = target.y.InvClamp(min.x, max.y);
            target.z = target.y.InvClamp(min.x, max.y);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3Int InvClamp(this Vector3Int target, int min, int max)
        {
            target.x = target.x.InvClamp(min, max);
            target.y = target.y.InvClamp(min, max);
            target.z = target.y.InvClamp(min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector3Int InvClamp(this Vector3Int target, Vector3Int min, Vector3Int max)
        {
            target.x = target.x.InvClamp(min.x, max.y);
            target.y = target.y.InvClamp(min.x, max.y);
            target.z = target.y.InvClamp(min.x, max.y);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using floats.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector4 InvClamp(this Vector4 target, float min, float max)
        {
            target.x = target.x.InvClamp(min, max);
            target.y = target.y.InvClamp(min, max);
            target.z = target.y.InvClamp(min, max);
            target.w = target.y.InvClamp(min, max);

            return target;
        }

        /// <summary>
        /// Clamps every vector values using vectors.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The clamped vector.</returns>
        public static Vector4 InvClamp(this Vector4 target, Vector4 min, Vector4 max)
        {
            target.x = target.x.InvClamp(min.x, max.y);
            target.y = target.y.InvClamp(min.x, max.y);
            target.z = target.y.InvClamp(min.x, max.y);
            target.w = target.y.InvClamp(min.x, max.y);

            return target;
        }

        #endregion

        #region Sign

        /// <summary>
        /// Return vector filled with its original values passed through Mathf.Sign.
        /// </summary>
        /// <returns>The sign vector.</returns>
        public static Vector2 Sign(this Vector2 target)
        {
            return new Vector2(target.x.Sign(), target.y.Sign());
        }

        /// <summary>
        /// Return vector filled with its original values passed through MathUtil.Sign.
        /// </summary>
        /// <returns>The sign vector.</returns>
        public static Vector2Int Sign(this Vector2Int target)
        {
            return new Vector2Int(target.x.Sign(), target.y.Sign());
        }

        /// <summary>
        /// Return vector filled with its original values passed through Mathf.Sign.
        /// </summary>
        /// <returns>The sign vector.</returns>
        public static Vector3 Sign(this Vector3 target)
        {
            return new Vector3(target.x.Sign(), target.y.Sign(), target.z.Sign());
        }

        /// <summary>
        /// Return vector filled with its original values passed through MathUtil.Sign.
        /// </summary>
        /// <returns>The sign vector.</returns>
        public static Vector3Int Sign(this Vector3Int target)
        {
            return new Vector3Int(target.x.Sign(), target.y.Sign(), target.z.Sign());
        }

        /// <summary>
        /// Return vector filled with its original values passed through Mathf.Sign.
        /// </summary>
        /// <returns>The sign vector.</returns>
        public static Vector4 Sign(this Vector4 target)
        {
            return new Vector4(target.x.Sign(), target.y.Sign(), target.z.Sign(), target.w.Sign());
        }

        #endregion

        #region SignedAdd

        /// <summary>
        /// Adds a value to the vector. If the vector's value is negative, the value added to the vector will be negative. Results in a value going away from 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector2 SignedAdd(this Vector2 target, float value)
        {
            target.x += value * target.x.Sign();
            target.y += value * target.y.Sign();

            return target;
        }

        /// <summary>
        /// Adds a value to the vector. If the vector's value is negative, the value added to the vector will be negative. Results in a value going away from 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector2Int SignedAdd(this Vector2Int target, int value)
        {
            target.x += value * target.x.Sign();
            target.y += value * target.y.Sign();

            return target;
        }

        /// <summary>
        /// Adds a value to the vector. If the vector's value is negative, the value added to the vector will be negative. Results in a value going away from 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector3 SignedAdd(this Vector3 target, float value)
        {
            target.x += value * target.x.Sign();
            target.y += value * target.y.Sign();
            target.z += value * target.z.Sign();

            return target;
        }

        /// <summary>
        /// Adds a value to the vector. If the vector's value is negative, the value added to the vector will be negative. Results in a value going away from 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector3Int SignedAdd(this Vector3Int target, int value)
        {
            target.x += value * target.x.Sign();
            target.y += value * target.y.Sign();
            target.z += value * target.z.Sign();

            return target;
        }

        /// <summary>
        /// Adds a value to the vector. If the vector's value is negative, the value added to the vector will be negative. Results in a value going away from 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector4 SignedAdd(this Vector4 target, float value)
        {
            target.x += value * target.x.Sign();
            target.y += value * target.y.Sign();
            target.z += value * target.z.Sign();
            target.w += value * target.w.Sign();

            return target;
        }

        #endregion

        #region SignedMinus

        /// <summary>
        /// Subtracts a value to the vector. If the vector's value is negative, the value subtracted to the vector will be negative. Results in a value going towards 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector2 SignedMinus(this Vector2 target, float value)
        {
            target.x -= value * target.x.Sign();
            target.y -= value * target.y.Sign();

            return target;
        }

        /// <summary>
        /// Subtracts a value to the vector. If the vector's value is negative, the value subtracted to the vector will be negative. Results in a value going towards 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector2Int SignedMinus(this Vector2Int target, int value)
        {
            target.x -= value * target.x.Sign();
            target.y -= value * target.y.Sign();

            return target;
        }

        /// <summary>
        /// Subtracts a value to the vector. If the vector's value is negative, the value subtracted to the vector will be negative. Results in a value going towards 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector3 SignedMinus(this Vector3 target, float value)
        {
            target.x -= value * target.x.Sign();
            target.y -= value * target.y.Sign();
            target.z -= value * target.z.Sign();

            return target;
        }

        /// <summary>
        /// Subtracts a value to the vector. If the vector's value is negative, the value subtracted to the vector will be negative. Results in a value going towards 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector3Int SignedMinus(this Vector3Int target, int value)
        {
            target.x -= value * target.x.Sign();
            target.y -= value * target.y.Sign();
            target.z -= value * target.z.Sign();

            return target;
        }

        /// <summary>
        /// Subtracts a value to the vector. If the vector's value is negative, the value subtracted to the vector will be negative. Results in a value going towards 0.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>The added vector.</returns>
        public static Vector4 SignedMinus(this Vector4 target, float value)
        {
            target.x -= value * target.x.Sign();
            target.y -= value * target.y.Sign();
            target.z -= value * target.z.Sign();
            target.w -= value * target.w.Sign();

            return target;
        }

        #endregion

        #region Subdivision

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 xy(this Vector3 target)
        {
            return new Vector2(target.x, target.y);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 xz(this Vector3 target)
        {
            return new Vector2(target.x, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 yz(this Vector3 target)
        {
            return new Vector2(target.y, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2Int xy(this Vector3Int target)
        {
            return new Vector2Int(target.x, target.y);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2Int xz(this Vector3Int target)
        {
            return new Vector2Int(target.x, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2Int yz(this Vector3Int target)
        {
            return new Vector2Int(target.y, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 xy(this Vector4 target)
        {
            return new Vector2(target.x, target.y);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 xz(this Vector4 target)
        {
            return new Vector2(target.x, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 xw(this Vector4 target)
        {
            return new Vector2(target.x, target.w);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 yz(this Vector4 target)
        {
            return new Vector2(target.y, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 yw(this Vector4 target)
        {
            return new Vector2(target.y, target.w);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector2 zw(this Vector4 target)
        {
            return new Vector2(target.z, target.w);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector3 xyz(this Vector4 target)
        {
            return new Vector3(target.x, target.y, target.z);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector3 xyw(this Vector4 target)
        {
            return new Vector3(target.x, target.y, target.w);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector3 xzw(this Vector4 target)
        {
            return new Vector3(target.x, target.z, target.w);
        }

        /// <summary>
        /// Returns a vector made with the requested variables.
        /// </summary>
        /// <returns>Subdivided vector.</returns>
        public static Vector3 yzw(this Vector4 target)
        {
            return new Vector3(target.y, target.z, target.w);
        }

        #endregion

        #region Closest Point

        /// <summary>
        /// Checks which given point is the closest to the original vector.
        /// </summary>
        /// <returns>The closest point.</returns>
        public static Vector2 ClosestPoint(this Vector2 target, params Vector2[] points)
        {
            Vector2 closest = Vector2.zero;
            float dist = float.MaxValue;
            foreach (Vector2 point in points)
            {
                float diff = Vector2.Distance(target, point);
                if (diff < dist)
                {
                    dist = diff;
                    closest = point;
                }
            }
            return closest;
        }

        /// <summary>
        /// Checks which given point is the closest to the original vector.
        /// </summary>
        /// <returns>The closest point.</returns>
        public static Vector2Int ClosestPoint(this Vector2Int target, params Vector2Int[] points)
        {
            Vector2Int closest = Vector2Int.zero;
            float dist = float.MaxValue;
            foreach (Vector2Int point in points)
            {
                float diff = Vector2Int.Distance(target, point);
                if (diff < dist)
                {
                    dist = diff;
                    closest = point;
                }
            }
            return closest;
        }

        /// <summary>
        /// Checks which given point is the closest to the original vector.
        /// </summary>
        /// <returns>The closest point.</returns>
        public static Vector3 ClosestPoint(this Vector3 target, params Vector3[] points)
        {
            Vector3 closest = Vector3.zero;
            float dist = float.MaxValue;
            foreach (Vector3 point in points)
            {
                float diff = Vector3.Distance(target, point);
                if (diff < dist)
                {
                    dist = diff;
                    closest = point;
                }
            }
            return closest;
        }

        /// <summary>
        /// Checks which given point is the closest to the original vector.
        /// </summary>
        /// <returns>The closest point.</returns>
        public static Vector3Int ClosestPoint(this Vector3Int target, params Vector3Int[] points)
        {
            Vector3Int closest = Vector3Int.zero;
            float dist = float.MaxValue;
            foreach (Vector3Int point in points)
            {
                float diff = Vector3Int.Distance(target, point);
                if (diff < dist)
                {
                    dist = diff;
                    closest = point;
                }
            }
            return closest;
        }

        #endregion

        #region Is Closest Point A

        /// <summary>
        /// Checks if pointA is closer to the original vector than pointB.
        /// </summary>If pointA is closer.</returns>
        public static bool IsClosestPointA(this Vector2 target, Vector2 pointA, Vector2 pointB)
        {
            return Vector2.Distance(target, pointA) <= Vector2.Distance(target, pointB);
        }

        /// <summary>
        /// Checks if pointA is closer to the original vector than pointB.
        /// </summary>If pointA is closer.</returns>
        public static bool IsClosestPointA(this Vector2Int target, Vector2Int pointA, Vector2Int pointB)
        {
            return Vector2Int.Distance(target, pointA) <= Vector2Int.Distance(target, pointB);
        }

        /// <summary>
        /// Checks if pointA is closer to the original vector than pointB.
        /// </summary>If pointA is closer.</returns>
        public static bool IsClosestPointA(this Vector3 target, Vector3 pointA, Vector3 pointB)
        {
            return Vector3.Distance(target, pointA) <= Vector3.Distance(target, pointB);
        }

        /// <summary>
        /// Checks if pointA is closer to the original vector than pointB.
        /// </summary>If pointA is closer.</returns>
        public static bool IsClosestPointA(this Vector3Int target, Vector3Int pointA, Vector3Int pointB)
        {
            return Vector3Int.Distance(target, pointA) <= Vector3Int.Distance(target, pointB);
        }

        #endregion

        #region To Quaternion

        /// <summary>
        /// Directly converts Vector4 to Quaternion.
        /// </summary>
        /// <returns>Quaternion made from Vector4.</returns>
        public static Quaternion Quaternion(this Vector4 target)
        {
            return new Quaternion(target.x, target.y, target.z, target.w);
        }

        /// <summary>
        /// Converts Vector4 to Quaternion after normalizing it.
        /// </summary>
        /// <returns>Quaternion made from Vector4.</returns>
        public static Quaternion NormalizedQuaternion(this Vector4 target)
        {
            target = Vector4.Normalize(target);
            return new Quaternion(target.x, target.y, target.z, target.w);
        }

        #endregion

        #region Rotate

        /// <summary>
        /// Rotate a 2D vector around 0,0.
        /// </summary>
        /// <param name="degrees">Angle in degrees.</param>
        /// <returns>The result vector.</returns>
        public static Vector2 Rotate(this Vector2 target, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = target.x;
            float ty = target.y;

            target.x = (cos * tx) - (sin * ty);
            target.y = (sin * tx) + (cos * ty);
            
            return target;
        }

        #endregion

        #region Unit Vector

        /// <summary>
        /// Returns the unit circle position of the given angle (in degree).
        /// </summary>
        /// <param name="angle">Angle in degree.</param>
        /// <returns>The result vector.</returns>
        public static Vector2 UnitVector(this Vector2 vector, float angle)
        {
            return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        #endregion
    }
}