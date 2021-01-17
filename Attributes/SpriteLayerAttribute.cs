// ====================================================================================================
//
// Sprite Layer Selector Attribute
//
// Original Code by Kaynn, Yeo Wen Qin [https://github.com/Kaynn-Cahya]
// Edited by Andrew Rumak [https://github.com/Deadcows] and Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

namespace Canty
{
    public class SpriteLayerAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(SpriteLayerAttribute))]
    public class SpriteLayerAttributeDrawer : PropertyDrawer
    {
        private bool m_CheckedType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                if (!m_CheckedType)
                {
                    Debug.LogWarning($"Property [{property.name}] in object [{property.serializedObject.targetObject}] is of wrong type. Expected: Int");
                    m_CheckedType = true;
                }

                EditorGUI.PropertyField(position, property, label);
                return;
            }

            string[] spriteLayerNames = GetSpriteLayerNames();
            HandleSpriteLayerSelectionUI(position, property, label, spriteLayerNames);
        }

        private void HandleSpriteLayerSelectionUI(Rect position, SerializedProperty property, GUIContent label, string[] spriteLayerNames)
        {
            EditorGUI.BeginProperty(position, label, property);

            // To show which sprite layer is currently selected.
            int currentSpriteLayerIndex;
            bool layerFound = TryGetSpriteLayerIndexFromProperty(out currentSpriteLayerIndex, spriteLayerNames, property);

            if (!layerFound)
            {
                // Set to default layer. (Previous layer was removed)
                Debug.Log($"Property [{property.name}] in object [{property.serializedObject.targetObject}] is set to the default layer. Reason: previously selected layer was removed.");
                property.intValue = 0;
                currentSpriteLayerIndex = 0;
            }

            int selectedSpriteLayerIndex = EditorGUI.Popup(position, label.text, currentSpriteLayerIndex, spriteLayerNames);

            // Change property value if user selects a new sprite layer.
            if (selectedSpriteLayerIndex != currentSpriteLayerIndex)
            {
                property.intValue = SortingLayer.NameToID(spriteLayerNames[selectedSpriteLayerIndex]);
            }

            EditorGUI.EndProperty();
        }

        private bool TryGetSpriteLayerIndexFromProperty(out int index, string[] spriteLayerNames, SerializedProperty property)
        {
            // To keep the property's value consistent, after the layers have been sorted around.
            string layerName = SortingLayer.IDToName(property.intValue);

            // Return the index where on it matches.
            for (int i = 0; i < spriteLayerNames.Length; ++i)
            {
                if (spriteLayerNames[i].Equals(layerName))
                {
                    index = i;
                    return true;
                }
            }

            // The current layer was removed.
            index = -1;
            return false;
        }

        private string[] GetSpriteLayerNames()
        {
            string[] result = new string[SortingLayer.layers.Length];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = SortingLayer.layers[i].name;
            }

            return result;
        }
    }
}
#endif