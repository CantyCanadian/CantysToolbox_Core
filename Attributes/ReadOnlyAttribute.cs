// ====================================================================================================
//
// Read Only Attribute
//
// Original Code by Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

namespace Canty
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute()
        {
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
#endif