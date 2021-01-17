// ====================================================================================================
//
// Display Inspector Attribute
//
// Original Code by Andrew Rumak [https://github.com/Deadcows]
// Edited by Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Canty
{
    /// <summary>
    /// Use to display inspector of a property object.
    /// </summary>
    public class DisplayInspectorAttribute : PropertyAttribute
    {
        public readonly bool DisplayScript;

        public DisplayInspectorAttribute(bool displayScriptField = true)
        {
            DisplayScript = displayScriptField;
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    [CustomPropertyDrawer(typeof(DisplayInspectorAttribute))]
    public class DisplayInspectorAttributeDrawer : PropertyDrawer
    {
        private DisplayInspectorAttribute Instance => m_Instance ?? (m_Instance = attribute as DisplayInspectorAttribute);
        private DisplayInspectorAttribute m_Instance;

        private ButtonMethodHandler m_ButtonMethods;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Instance.DisplayScript || property.objectReferenceValue == null)
            {
                position.height = EditorGUI.GetPropertyHeight(property);
                EditorGUI.PropertyField(position, property, label);
                position.y += EditorGUI.GetPropertyHeight(property) + 4;
            }

            if (property.objectReferenceValue != null)
            {
                if (m_ButtonMethods == null)
                {
                    m_ButtonMethods = new ButtonMethodHandler(property.objectReferenceValue);
                }

                Vector2 start = new Vector2(position.x, position.y);

                SerializedProperty propertyObject = new SerializedObject(property.objectReferenceValue).GetIterator();
                propertyObject.Next(true);
                propertyObject.NextVisible(false);

                float xPos = position.x + 10;
                float width = position.width - 10;

                while (propertyObject.NextVisible(propertyObject.isExpanded))
                {
                    position.x = xPos + 10 * propertyObject.depth;
                    position.width = width - 10 * propertyObject.depth;

                    position.height = propertyObject.isExpanded ? 16 : EditorGUI.GetPropertyHeight(propertyObject);
                    EditorGUI.PropertyField(position, propertyObject);
                    position.y += propertyObject.isExpanded ? 20 : EditorGUI.GetPropertyHeight(propertyObject) + 4;
                }

                if (!(m_ButtonMethods.TargetMethods == null || m_ButtonMethods.TargetMethods.Count == 0))
                {
                    foreach ((System.Reflection.MethodInfo Method, string Name, ButtonMethodAttribute.ButtonMethodDrawOrder Order) in m_ButtonMethods.TargetMethods)
                    {
                        position.height = EditorGUIUtility.singleLineHeight;
                        if (GUI.Button(position, Name)) m_ButtonMethods.Invoke(Method);
                        position.y += position.height;
                    }
                }

                var bgRect = position;
                bgRect.x = start.x - 10;
                bgRect.y = start.y - 5;
                bgRect.width = 10;
                bgRect.height = position.y - start.y;

                if (m_ButtonMethods.Amount > 0)
                {
                    bgRect.height += 5;
                }

                DrawColouredRect(bgRect, new Color(0.6f, 0.6f, 0.8f, 0.5f));

                if (GUI.changed)
                {
                    propertyObject.serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUI.changed)
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                return GetPropertyHeight(property, label);
            }

            if (m_ButtonMethods == null)
            {
                m_ButtonMethods = new ButtonMethodHandler(property.objectReferenceValue);
            }

            float height = Instance.DisplayScript ? EditorGUI.GetPropertyHeight(property) + 4 : 0;

            SerializedProperty propertyObject = new SerializedObject(property.objectReferenceValue).GetIterator();
            propertyObject.Next(true);
            propertyObject.NextVisible(true);

            while (propertyObject.NextVisible(propertyObject.isExpanded))
            {
                height += propertyObject.isExpanded ? 20 : EditorGUI.GetPropertyHeight(propertyObject) + 4;
            }

            if (m_ButtonMethods.Amount > 0)
            {
                height += 4 + m_ButtonMethods.Amount * EditorGUIUtility.singleLineHeight;
            }

            return height;
        }

        private void DrawColouredRect(Rect rect, Color color)
        {
            Color defaultBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(rect, "");
            GUI.backgroundColor = defaultBackgroundColor;
        }
    }
}
#endif