// ====================================================================================================
//
// Must be Assigned Attribute
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
    /// Apply to MonoBehaviour field to assert that this field is assigned via inspector (not null, false, empty of zero) on playmode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MustBeAssignedAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.Experimental.SceneManagement;

    [InitializeOnLoad]
    public class MustBeAssignedAttributeChecker
    {
        /// <summary>
        /// A way to conditionally disable MustBeAssigned check
        /// </summary>
        public static Func<FieldInfo, MonoBehaviour, bool> ExcludeFieldFilter;

        static MustBeAssignedAttributeChecker()
        {
            EditorEvents.OnSave += AssertComponentsInScene;
            PrefabStage.prefabSaved += AssertComponentsInPrefab;
        }

        private static void AssertComponentsInScene()
        {
            MonoBehaviour[] components = Canty.EditorUtil.GetAllBehavioursInScenes();
            AssertComponent(components);
        }

        private static void AssertComponentsInPrefab(GameObject prefab)
        {
            MonoBehaviour[] components = prefab.GetComponentsInChildren<MonoBehaviour>();
            AssertComponent(components);
        }

        private static void AssertComponent(MonoBehaviour[] components)
        {
            foreach (MonoBehaviour behaviour in components)
            {
                if (behaviour == null)
                {
                    continue;
                }

                Type typeOfScript = behaviour.GetType();
                FieldInfo[] mustBeAssignedFields = typeOfScript
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(field => field.IsDefined(typeof(MustBeAssignedAttribute), false)).ToArray();

                foreach (FieldInfo field in mustBeAssignedFields)
                {
                    object propValue = field.GetValue(behaviour);

                    // Used by external systems to exclude specific fields.
                    // Specifically for ConditionalFieldAttribute
                    if (FieldIsExcluded(field, behaviour))
                    {
                        continue;
                    }
                    
                    if (field.FieldType.IsValueType && Activator.CreateInstance(field.FieldType).Equals(propValue))
                    {
                        Debug.LogError($"{typeOfScript.Name} caused: {field.Name} is Value Type with default value", behaviour.gameObject);
                        continue;
                    }
                    
                    if (propValue == null || propValue.Equals(null))
                    {
                        Debug.LogError($"{typeOfScript.Name} caused: {field.Name} is not assigned (null value)", behaviour.gameObject);
                        continue;
                    }
                    
                    if (field.FieldType == typeof(string) && (string)propValue == string.Empty)
                    {
                        Debug.LogError($"{typeOfScript.Name} caused: {field.Name} is not assigned (empty string)", behaviour.gameObject);
                        continue;
                    }
                    
                    Array arr = propValue as Array;
                    if (arr != null && arr.Length == 0)
                    {
                        Debug.LogError($"{typeOfScript.Name} caused: {field.Name} is not assigned (empty array)", behaviour.gameObject);
                    }
                }
            }
        }

        private static bool FieldIsExcluded(FieldInfo field, MonoBehaviour behaviour)
        {
            if (ExcludeFieldFilter == null)
            {
                return false;
            }

            foreach (var filterDelegate in ExcludeFieldFilter.GetInvocationList())
            {
                Func<FieldInfo, MonoBehaviour, bool> filter = filterDelegate as Func<FieldInfo, MonoBehaviour, bool>;
                if (filter != null && filter(field, behaviour))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif