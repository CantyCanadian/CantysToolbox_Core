// ====================================================================================================
//
// Defined Values Attribute
//
// Original Code by Andrew Rumak [https://github.com/Deadcows]
// Edited by Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using System;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Create Popup with predefined values for string, int or float property.
    /// </summary>
    public class DefinedValuesAttribute : PropertyAttribute
    {
        public readonly object[] ValuesArray;

        public DefinedValuesAttribute(params object[] definedValues)
        {
            ValuesArray = definedValues;
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(DefinedValuesAttribute))]
    public class DefinedValuesAttributeDrawer : PropertyDrawer
    {
        private DefinedValuesAttribute m_Attribute;
        private Type m_VariableType;
        private string[] m_Values;
        private int m_SelectedValueIndex = -1;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_Attribute == null)
            {
                Initialize(property);
            }

            if (m_Values == null || m_Values.Length == 0 || m_SelectedValueIndex < 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginChangeCheck();
            m_SelectedValueIndex = EditorGUI.Popup(position, label.text, m_SelectedValueIndex, m_Values);
            if (EditorGUI.EndChangeCheck())
            {
                ApplyNewValue(property);
            }
        }

        private void Initialize(SerializedProperty property)
        {
            m_Attribute = attribute as DefinedValuesAttribute;
            if (m_Attribute.ValuesArray == null || m_Attribute.ValuesArray.Length == 0)
            {
                return;
            }

            m_VariableType = m_Attribute.ValuesArray[0].GetType();
            if (TypeMismatch(property))
            {
                return;
            }
            
            m_Values = new string[m_Attribute.ValuesArray.Length];
            for (int i = 0; i < m_Attribute.ValuesArray.Length; i++)
            {
                m_Values[i] = m_Attribute.ValuesArray[i].ToString();
            }

            m_SelectedValueIndex = GetSelectedIndex(property);
        }

        private int GetSelectedIndex(SerializedProperty property)
        {
            for (var i = 0; i < m_Values.Length; i++)
            {
                if (IsString && property.stringValue == m_Values[i])
                {
                    return i;
                }

                if (IsInt && property.intValue == Convert.ToInt32(m_Values[i]))
                {
                    return i;
                }

                if (IsFloat && Mathf.Approximately(property.floatValue, Convert.ToSingle(m_Values[i])))
                {
                    return i;
                }
            }

            return 0;
        }

        private bool TypeMismatch(SerializedProperty property)
        {
            if (IsString && property.propertyType != SerializedPropertyType.String)
            {
                return true;
            }

            if (IsInt && property.propertyType != SerializedPropertyType.Integer)
            {
                return true;
            }

            if (IsFloat && property.propertyType != SerializedPropertyType.Float)
            {
                return true;
            }

            return false;
        }

        private void ApplyNewValue(SerializedProperty property)
        {
            if (IsString)
            {
                property.stringValue = m_Values[m_SelectedValueIndex];
            }

            else if (IsInt)
            {
                property.intValue = Convert.ToInt32(m_Values[m_SelectedValueIndex]);
            }

            else if (IsFloat)
            {
                property.floatValue = Convert.ToSingle(m_Values[m_SelectedValueIndex]);
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        private bool IsString
        {
            get { return m_VariableType == typeof(string); }
        }

        private bool IsInt
        {
            get { return m_VariableType == typeof(int); }
        }

        private bool IsFloat
        {
            get { return m_VariableType == typeof(float); }
        }
    }
}
#endif