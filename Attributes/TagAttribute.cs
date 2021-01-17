// ====================================================================================================
//
// Tag Attribute
//
// Original Code by Kaynn, Yeo Wen Qin [https://github.com/Kaynn-Cahya]
// Edited by Andrew Rumak [https://github.com/Deadcows] and Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

namespace Canty
{
    public class TagAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttributeDrawer : PropertyDrawer
    {
        private bool m_Checked;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                if (!m_Checked)
                {
                    Debug.LogWarning($"Property [{property.name}] in object [{property.serializedObject.targetObject}] is of wrong type. Expected: String");
                    m_Checked = true;
                }

                EditorGUI.PropertyField(position, property, label);
                return;
            }

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}
#endif