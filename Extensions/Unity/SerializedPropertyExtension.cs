#if UNITY_EDITOR

using System;
using UnityEditor;

namespace Canty
{
    public static class SerializedPropertyExtension
    {
        /// <summary>
        /// Get string representation of serialized property.
        /// </summary>
        public static string AsStringValue(this SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    return property.stringValue;

                case SerializedPropertyType.Character:
                case SerializedPropertyType.Integer:
                    if (property.type == "char") return Convert.ToChar(property.intValue).ToString();
                    return property.intValue.ToString();

                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue != null ? property.objectReferenceValue.ToString() : "null";

                case SerializedPropertyType.Boolean:
                    return property.boolValue.ToString();

                case SerializedPropertyType.Enum:
                    return property.enumNames[property.enumValueIndex];

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Check if property is float, int, vector or int vector.
        /// </summary>
        public static bool IsNumerical(this SerializedProperty property)
        {
            SerializedPropertyType propertyType = property.propertyType;
            switch (propertyType)
            {
                case SerializedPropertyType.Float:
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                    return true;

                default:
                    return false;
            }
        }
    }
}

#endif