// ====================================================================================================
//
// Conditional Field Attribute
//
// Original Code by Andrew Rumak [https://github.com/Deadcows]
// Edited by Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Canty
{
    /// <summary>
    /// Conditionally Show/Hide field in inspector, based on some other field value 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string FieldToCheck;
        public readonly string[] CompareValues;
        public readonly bool Inverse;

        /// <param name="fieldToCheck">String name of field to check value</param>
        /// <param name="inverse">Inverse check result</param>
        /// <param name="compareValues">On which values field will be shown in inspector</param>
        public ConditionalFieldAttribute(string fieldToCheck, bool inverse = false, params object[] compareValues)
        {
            FieldToCheck = fieldToCheck;
            Inverse = inverse;
            CompareValues = compareValues.Select(c => c.ToString().ToUpper()).ToArray();
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Making sure the attribute is obtained and stored when called.
        /// </summary>
        private ConditionalFieldAttribute Conditional { get { return m_Conditional ?? (m_Conditional = attribute as ConditionalFieldAttribute); } }
        private ConditionalFieldAttribute m_Conditional;

        private bool m_CustomDrawersCached;
        private static IEnumerable<Type> m_AllPropertyDrawerAttributeTypes;
        private bool m_MultipleAttributes;
        private bool m_SpecialType;
        private PropertyAttribute m_GenericAttribute;
        private PropertyDrawer m_GenericAttributeDrawerInstance;
        private Type m_GenericAttributeDrawerType;
        private Type m_GenericType;
        private PropertyDrawer m_GenericTypeDrawerInstance;
        private Type m_GenericTypeDrawerType;
        
        /// <summary>
        /// Cache the connections between the tagged attribute and the target attribute.
        /// </summary>
        private readonly Dictionary<SerializedProperty, SerializedProperty> m_ConditionalToTarget = new Dictionary<SerializedProperty, SerializedProperty>();

        private bool m_ToShow = true;


        private void Initialize(SerializedProperty property)
        {
            if (!m_ConditionalToTarget.ContainsKey(property))
            {
                m_ConditionalToTarget.Add(property, ConditionalFieldUtility.FindRelativeProperty(property, Conditional.FieldToCheck));
            }

            if (m_CustomDrawersCached)
            {
                return;
            }

            if (m_AllPropertyDrawerAttributeTypes == null)
            {
                m_AllPropertyDrawerAttributeTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => typeof(PropertyDrawer).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            }

            if (HaveMultipleAttributes())
            {
                m_MultipleAttributes = true;
                GetPropertyDrawerType(property);
            }
            else if (fieldInfo != null && !fieldInfo.FieldType.Module.ScopeName.Equals(typeof(int).Module.ScopeName))
            {
                m_SpecialType = true;
                GetTypeDrawerType(property);
            }

            m_CustomDrawersCached = true;
        }

        private bool HaveMultipleAttributes()
        {
            if (fieldInfo == null)
            {
                return false;
            }

            Type genericAttributeType = typeof(PropertyAttribute);
            object[] attributes = fieldInfo.GetCustomAttributes(genericAttributeType, false);

            if (attributes != null && attributes.Length == 0)
            {
                return false;
            }

            return attributes.Length > 1;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Initialize(property);

            m_ToShow = ConditionalFieldUtility.PropertyIsVisible(m_ConditionalToTarget[property], Conditional.Inverse, Conditional.CompareValues);

            if (!m_ToShow)
            {
                return 0.0f;
            }

            if (m_GenericAttributeDrawerInstance != null)
            {
                return m_GenericAttributeDrawerInstance.GetPropertyHeight(property, label);
            }

            if (m_GenericTypeDrawerInstance != null)
            {
                return m_GenericTypeDrawerInstance.GetPropertyHeight(property, label);
            }

            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!m_ToShow)
            {
                return;
            }

            if (m_MultipleAttributes && m_GenericAttributeDrawerInstance != null)
            {
                try
                {
                    m_GenericAttributeDrawerInstance.OnGUI(position, property, label);
                }
                catch (Exception e)
                {
                    EditorGUI.PropertyField(position, property, label);
                    Debug.LogWarning($"Unable to instantiate [{m_GenericAttribute.GetType()}] due to exception [{e}].", property.serializedObject.targetObject);
                }
            }
            else if (m_SpecialType && m_GenericTypeDrawerInstance != null)
            {
                try
                {
                    m_GenericTypeDrawerInstance.OnGUI(position, property, label);
                }
                catch (Exception e)
                {
                    EditorGUI.PropertyField(position, property, label);
                    Debug.LogWarning($"Unable to instantiate [{m_GenericType}] due to exception [{e}].", property.serializedObject.targetObject);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private void GetPropertyDrawerType(SerializedProperty property)
        {
            if (m_GenericAttributeDrawerInstance != null)
            {
                return;
            }

            // If the target attribute has a tag, we have to make sure we can show/hide it.
            try
            {
                m_GenericAttribute = (PropertyAttribute)fieldInfo.GetCustomAttributes(typeof(PropertyAttribute), false).FirstOrDefault(a => !(a is ConditionalFieldAttribute));
                
                if (m_GenericAttribute is ContextMenuItemAttribute || m_GenericAttribute is SeparatorAttribute)
                {
                    Debug.LogWarning($"ConditionalField does not work with [{m_GenericAttribute.GetType()}].", property.serializedObject.targetObject);
                    return;
                }

                // It's normal to want to hide a member with a tooltip, so we return without warning.
                if (m_GenericAttribute is TooltipAttribute)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Can't find stacked propertyAttribute after ConditionalProperty [{e}].", property.serializedObject.targetObject);
                return;
            }

            // Get the associated attribute drawer for the target member's attribute.
            try
            {
                m_GenericAttributeDrawerType = m_AllPropertyDrawerAttributeTypes.First(x => (Type)CustomAttributeData.GetCustomAttributes(x).First().ConstructorArguments.First().Value == m_GenericAttribute.GetType());
            }
            catch (Exception e)
            {
                Debug.LogWarning("Can't find property drawer from CustomPropertyAttribute of " + m_GenericAttribute.GetType() + " : " + e, property.serializedObject.targetObject);
                return;
            }
            
            // Instantiate the target member's attribute so that we can show and hide its GUI code alongside the target property.
            try
            {
                m_GenericAttributeDrawerInstance = Activator.CreateInstance(m_GenericAttributeDrawerType) as PropertyDrawer;

                IList<CustomAttributeTypedArgument> attributeParams = fieldInfo.GetCustomAttributesData().First(a => a.AttributeType == m_GenericAttribute.GetType()).ConstructorArguments;
                IList<CustomAttributeTypedArgument> unpackedParams = new List<CustomAttributeTypedArgument>();

                foreach (CustomAttributeTypedArgument singleParam in attributeParams)
                {
                    if (singleParam.Value.GetType() == typeof(ReadOnlyCollection<CustomAttributeTypedArgument>))
                    {
                        foreach (CustomAttributeTypedArgument unpackedSingleParam in (ReadOnlyCollection<CustomAttributeTypedArgument>)singleParam.Value)
                        {
                            unpackedParams.Add(unpackedSingleParam);
                        }
                    }
                    else
                    {
                        unpackedParams.Add(singleParam);
                    }
                }

                object[] attributeParamsObj = unpackedParams.Select(x => x.Value).ToArray();

                if (attributeParamsObj.Any())
                {
                    m_GenericAttribute = Activator.CreateInstance(m_GenericAttribute.GetType(), attributeParamsObj) as PropertyAttribute;
                }
                else
                {
                    m_GenericAttribute = Activator.CreateInstance(m_GenericAttribute.GetType()) as PropertyAttribute;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"No constructor available in [{m_GenericAttribute.GetType()}] : [{e}].", property.serializedObject.targetObject);
                return;
            }

            // Copy the data from the target member's attribute.
            try
            {
                FieldInfo genericDrawerAttributeField = m_GenericAttributeDrawerType.GetField("m_Attribute", BindingFlags.Instance | BindingFlags.NonPublic);
                genericDrawerAttributeField.SetValue(m_GenericAttributeDrawerInstance, m_GenericAttribute);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Unable to assign attribute to [{m_GenericAttributeDrawerInstance.GetType()}] : [{e}].", property.serializedObject.targetObject);
            }
        }

        private void GetTypeDrawerType(SerializedProperty property)
        {
            if (m_GenericTypeDrawerInstance != null)
            {
                return;
            }

            // Get the associated attribute drawer.
            try
            {
                // Of all property drawers in the assembly we need to find one that affects target type or one of the base types of target type.
                foreach (Type propertyDrawerType in m_AllPropertyDrawerAttributeTypes)
                {
                    m_GenericType = fieldInfo.FieldType;
                    Type affectedType = CustomAttributeData.GetCustomAttributes(propertyDrawerType).First().ConstructorArguments.First().Value as Type;
                    while (m_GenericType != null)
                    {
                        if (m_GenericTypeDrawerType != null)
                        {
                            break;
                        }

                        if (affectedType == m_GenericType)
                        {
                            m_GenericTypeDrawerType = propertyDrawerType;
                        }
                        else
                        {
                            m_GenericType = m_GenericType.BaseType;
                        }
                    }

                    if (m_GenericTypeDrawerType != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // No warnings here cuz it can happen even in normal usage.
                return;
            }

            if (m_GenericTypeDrawerType == null)
            {
                return;
            }

            // Create an instance of the type drawer so we can show/hide it.
            try
            {
                m_GenericTypeDrawerInstance = Activator.CreateInstance(m_GenericTypeDrawerType) as PropertyDrawer;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"No constructor available in [{m_GenericType}] : [{e}].", property.serializedObject.targetObject);
                return;
            }

            // Copy the values from the target member to our instance.
            try
            {
                m_GenericTypeDrawerType.GetField("m_Attribute", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(m_GenericTypeDrawerInstance, fieldInfo);
            }
            catch
            {
                // No warnings here cuz it can happen even in normal usage.
            }
        }
    }

    public static class ConditionalFieldUtility
    {
        public static bool PropertyIsVisible(SerializedProperty property, bool inverse, string[] compareAgainst)
        {
            if (property == null)
            {
                return true;
            }

            string asString = property.AsStringValue().ToUpper();

            if (compareAgainst != null && compareAgainst.Length > 0)
            {
                bool matchAny = CompareAgainstValues(asString, compareAgainst);

                if (inverse)
                {
                    matchAny = !matchAny;
                }

                return matchAny;
            }

            bool someValueAssigned = asString != "FALSE" && asString != "0" && asString != "NULL";
            if (someValueAssigned)
            {
                return !inverse;
            }

            return inverse;
        }

        /// <summary>
        /// True if the property value matches any of the values in 'm_CompareValues'
        /// </summary>
        /// <returns>If it matches.</returns>
        private static bool CompareAgainstValues(string propertyValueAsString, string[] compareAgainst)
        {
            for (int i = 0; i < compareAgainst.Length; i++)
            {
                bool valueMatches = compareAgainst[i] == propertyValueAsString;
                
                if (valueMatches)
                {
                    return true;
                }
            }
            
            return false;
        }

        public static SerializedProperty FindRelativeProperty(SerializedProperty property, string propertyName)
        {
            if (property.depth == 0)
            {
                return property.serializedObject.FindProperty(propertyName);
            }

            string path = property.propertyPath.Replace(".Array.data[", "[");
            string[] elements = path.Split('.');

            SerializedProperty nestedProperty = NestedPropertyOrigin(property, elements);

            // If nested property is null, we hit an array property.
            if (nestedProperty == null)
            {
                string cleanPath = path.Substring(0, path.IndexOf('['));
                SerializedProperty arrayProp = property.serializedObject.FindProperty(cleanPath);
                UnityEngine.Object target = arrayProp.serializedObject.targetObject;

                string who = $"Property [{arrayProp.name}] in object [{target.name}] caused: ";
                string warning = who + $"{who} Array fields is not supported by [ConditionalFieldAttribute]";

                Debug.LogWarning(warning, property.serializedObject.targetObject);

                return null;
            }

            return nestedProperty.FindPropertyRelative(propertyName);
        }

        // For [Serialized] types with [Conditional] fields.
        private static SerializedProperty NestedPropertyOrigin(SerializedProperty property, string[] elements)
        {
            SerializedProperty parent = null;

            for (int i = 0; i < elements.Length - 1; i++)
            {
                string element = elements[i];
                int index = -1;
                if (element.Contains("["))
                {
                    index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal)).Replace("[", "").Replace("]", ""));
                    element = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
                }

                parent = i == 0
                    ? property.serializedObject.FindProperty(element)
                    : parent != null
                        ? parent.FindPropertyRelative(element)
                        : null;

                if (index >= 0 && parent != null) parent = parent.GetArrayElementAtIndex(index);
            }

            return parent;
        }

        public static bool BehaviourPropertyIsVisible(MonoBehaviour behaviour, string propertyName, ConditionalFieldAttribute appliedAttribute)
        {
            if (string.IsNullOrEmpty(appliedAttribute.FieldToCheck)) return true;

            SerializedObject so = new SerializedObject(behaviour);
            SerializedProperty property = so.FindProperty(propertyName);
            SerializedProperty targetProperty = FindRelativeProperty(property, appliedAttribute.FieldToCheck);

            return PropertyIsVisible(targetProperty, appliedAttribute.Inverse, appliedAttribute.CompareValues);
        }
    }
}
#endif