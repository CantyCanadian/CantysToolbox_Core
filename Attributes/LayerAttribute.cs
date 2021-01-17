// ====================================================================================================
//
// Layer Attribute
//
// Original Code by Andrew Rumak [https://github.com/Deadcows]
// Edited by Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

namespace Canty
{
    public class LayerAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeDrawer : PropertyDrawer
    {
        private bool m_Checked;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                if (!m_Checked)
                {
                    Debug.LogWarning($"Property [{property.name}] in object [{property.serializedObject.targetObject}] is of wrong type. Expected: Int");
                    m_Checked = true;
                }

                EditorGUI.PropertyField(position, property, label);
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}
#endif