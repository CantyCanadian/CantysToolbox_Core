// ====================================================================================================
//
// Constant Selection Attribute
//
// Original Code by Andrew Rumak [https://github.com/Deadcows]
// Edited by Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Canty
{
    public class ConstantsSelectionAttribute : PropertyAttribute
    {
        public readonly Type SelectFromType;

        public ConstantsSelectionAttribute(Type type)
        {
            SelectFromType = type;
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ConstantsSelectionAttribute))]
    public class ConstantsSelectionAttributeDrawer : PropertyDrawer
    {
        private ConstantsSelectionAttribute m_Attribute;
        private readonly List<MemberInfo> m_Constants = new List<MemberInfo>();
        private List<string> m_Names;
        private List<object> m_Values;
        private Type m_TargetType;
        private int m_SelectedValueIndex;
        private bool m_ValueFound;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_Attribute == null)
            {
                Initialize(property);
            }

            if (m_Values == null || m_Values.Count == 0 || m_SelectedValueIndex < 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginChangeCheck();
            m_SelectedValueIndex = EditorGUI.Popup(position, label.text, m_SelectedValueIndex, m_Names.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                fieldInfo.SetValue(property.serializedObject.targetObject, m_Values[m_SelectedValueIndex]);
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }

        private object GetValue(SerializedProperty property)
        {
            return fieldInfo.GetValue(property.serializedObject.targetObject);
        }

        private void Initialize(SerializedProperty property)
        {
            m_Attribute = (ConstantsSelectionAttribute)attribute;
            m_TargetType = fieldInfo.FieldType;

            BindingFlags searchFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            FieldInfo[] allPublicStaticFields = m_Attribute.SelectFromType.GetFields(searchFlags);
            PropertyInfo[] allPublicStaticProperties = m_Attribute.SelectFromType.GetProperties(searchFlags);

            // IsLiteral determines if its value is written at compile time and not changeable
            // IsInitOnly determines if the field can be set in the body of the constructor for C# a field which is readonly keyword would have both true but a const field would have only IsLiteral equal to true.
            foreach (FieldInfo field in allPublicStaticFields)
            {
                if ((field.IsInitOnly || field.IsLiteral) && field.FieldType == m_TargetType)
                {
                    m_Constants.Add(field);
                }
            }

            foreach (PropertyInfo prop in allPublicStaticProperties)
            {
                if (prop.PropertyType == m_TargetType)
                {
                    m_Constants.Add(prop);
                }
            }
            
            if (m_Constants != null || m_Constants.Count == 0)
            {
                return;
            }

            m_Names = new List<string>();
            m_Values = new List<object>();
            for (var i = 0; i < m_Constants.Count; i++)
            {
                m_Names[i] = m_Constants[i].Name;
                m_Values[i] = GetValue(i);
            }

            object currentValue = GetValue(property);
            if (currentValue != null)
            {
                for (int i = 0; i < m_Values.Count; i++)
                {
                    if (currentValue.Equals(m_Values[i]))
                    {
                        m_ValueFound = true;
                        m_SelectedValueIndex = i;
                    }
                }
            }

            if (!m_ValueFound)
            {
                object actualValue = GetValue(property);
                object value = actualValue != null ? actualValue : "NULL";
                m_Names.Insert(0, "NOT FOUND: " + value);
                m_Values.Insert(0, actualValue);
            }
        }

        private object GetValue(int index)
        {
            MemberInfo member = m_Constants[index];
            if (member.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)member).GetValue(null);
            }

            if (member.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)member).GetValue(null);
            }

            return null;
        }
    }
}
#endif