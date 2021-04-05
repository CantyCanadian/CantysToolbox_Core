///====================================================================================================
///
///     SceneReference by JohannesMP, original repo at [https://github.com/JohannesMP/unity-scene-reference]
///
///====================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
#endif

// A wrapper that provides the means to safely serialize Scene Asset References.
//
// Internally we serialize an Object to the SceneAsset which only exists at editor time.
// Any time the object is serialized, we store the path provided by this Asset (assuming it was valid).
//
// This means that, come build time, the string path of the scene asset is always already stored, which if 
// the scene was added to the build settings means it can be loaded.
//
// It is up to the user to ensure the scene exists in the build settings so it is loadable at runtime.
// To help with this, a custom PropertyDrawer displays the scene build settings state.
//
//  Known issues:
// - When reverting back to a prefab which has the asset stored as null, Unity will show the property 
// as modified despite having just reverted. This only happens on the fist time, and reverting again fix it. 
// Under the hood the state is still always valid and serialized correctly regardless.


/// <summary>
/// A wrapper that provides the means to safely serialize Scene Asset References.
/// </summary>
[Serializable]
public class SceneReference : ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    // What we use in editor to select the scene.
    [SerializeField] private Object _sceneAsset;

    private bool IsValidSceneAsset
    {
        get
        {
            if (!_sceneAsset)
                return false;
            return _sceneAsset is SceneAsset;
        }
    }
#endif

    // This should only ever be set during serialization/deserialization.
    [SerializeField] private string _scenePath = string.Empty;

    /// <summary>
    /// Use this when you want to actually have the scene path.
    /// </summary>
    public string ScenePath
    {
        get
        {
#if UNITY_EDITOR
            // In editor we always use the asset's path
            return GetScenePathFromAsset();
#else
            // At runtime we rely on the stored path value which we assume was serialized correctly at build time.
            // See OnBeforeSerialize and OnAfterDeserialize.
            return scenePath;
#endif
        }
        set
        {
            _scenePath = value;
#if UNITY_EDITOR
            _sceneAsset = GetSceneAssetFromPath();
#endif
        }
    }

    public static implicit operator string(SceneReference sceneReference)
    {
        return sceneReference.ScenePath;
    }

    /// <summary>
    /// Called to prepare this data for serialization. Stubbed out when not in editor.
    /// </summary>
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        HandleBeforeSerialize();
#endif
    }

    /// <summary>
    /// Called to set up data for deserialization. Stubbed out when not in editor.
    /// </summary>
    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        // We sadly cannot touch assetdatabase during serialization, so defer by a bit.
        EditorApplication.update += HandleAfterDeserialize;
#endif
    }



#if UNITY_EDITOR
    private SceneAsset GetSceneAssetFromPath()
    {
        return string.IsNullOrEmpty(_scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(_scenePath);
    }

    private string GetScenePathFromAsset()
    {
        return _sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(_sceneAsset);
    }

    private void HandleBeforeSerialize()
    {
        if (IsValidSceneAsset == false && string.IsNullOrEmpty(_scenePath) == false)
        {
            // Asset is invalid but we have a path to try and recover it from.
            _sceneAsset = GetSceneAssetFromPath();

            if (_sceneAsset == null)
                _scenePath = string.Empty;

            EditorSceneManager.MarkAllScenesDirty();
        }
        else
        {
            // Asset takes precendence and overwrites the path.
            _scenePath = GetScenePathFromAsset();
        }
    }

    private void HandleAfterDeserialize()
    {
        EditorApplication.update -= HandleAfterDeserialize;

        // Asset is valid, don't do anything. Path will always be set based on it when it matters.
        if (IsValidSceneAsset)
            return;

        // Asset is invalid but we have a path to try and recover it from.
        if (string.IsNullOrEmpty(_scenePath))
            return;

        _sceneAsset = GetSceneAssetFromPath();

        // No asset found and path was invalid. We have to make sure we don't carry over the old, invalid path.
        if (!_sceneAsset)
            _scenePath = string.Empty;

        if (!Application.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();
    }
#endif
}

#if UNITY_EDITOR
/// <summary>
/// Display a SceneReference object in the editor.
/// If scene is valid, we provides basic buttons to interact with the scene's role in Build Settings.
/// </summary>
[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferencePropertyDrawer : PropertyDrawer
{
    // The exact name of the asset Object variable in the SceneReference object.
    private const string SCENE_ASSET_PROPERTY_STRING = "_sceneAsset";
    // The exact name of the scene Path variable in the SceneReference object.
    private const string SCENE_PATH_PROPERTY_STRING = "_scenePath";

    private static readonly RectOffset _boxPadding = EditorStyles.helpBox.padding;


    // Made these two const btw
    private const float PAD_SIZE = 2f;
    private const float FOOTER_HEIGHT = 2f;

    private static readonly float _lineHeight = EditorGUIUtility.singleLineHeight;
    private static readonly float _paddedLine = _lineHeight + PAD_SIZE;

    /// <summary>
    /// Drawing the 'SceneReference' property.
    /// </summary>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);
        {
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, _lineHeight), property.isExpanded, label);
            
            if (property.isExpanded)
            {
                // Reduce the height by one line and move the content one line below.
                position.height -= _lineHeight;
                position.y += _lineHeight;

                SerializedProperty sceneAssetProperty = GetSceneAssetProperty(property);

                // Draw the box background.
                position.height += FOOTER_HEIGHT;
                GUI.Box(EditorGUI.IndentedRect(position), GUIContent.none, EditorStyles.helpBox);
                position = _boxPadding.Remove(position);
                position.height = _lineHeight;
                
                int sceneControlID = GUIUtility.GetControlID(FocusType.Passive);
                EditorGUI.BeginChangeCheck();
                {
                    // removed the label here since we already have it in the foldout before.
                    sceneAssetProperty.objectReferenceValue = EditorGUI.ObjectField(position, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);
                }

                var buildScene = BuildUtils.GetBuildScene(sceneAssetProperty.objectReferenceValue);
                if (EditorGUI.EndChangeCheck())
                {
                    // If no valid scene asset was selected, reset the stored path accordingly.
                    if (buildScene.scene == null)
                        GetScenePathProperty(property).stringValue = string.Empty;
                }

                position.y += _paddedLine;

                if (!buildScene.assetGUID.Empty())
                {
                    // Draw the build settings info of the selected Scene.
                    DrawSceneInfoGUI(position, buildScene, sceneControlID + 1);
                }
            }
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty sceneAssetProperty = GetSceneAssetProperty(property);

        int lines = property.isExpanded ? (sceneAssetProperty.objectReferenceValue != null ? 3 : 2) : 1;

        return _lineHeight * lines + PAD_SIZE * (lines - 1) + FOOTER_HEIGHT;
    }

    /// <summary>
    /// Draws the info box of the provided Scene.
    /// </summary>
    private void DrawSceneInfoGUI(Rect position, BuildUtils.BuildScene buildScene, int sceneControlID)
    {
        bool readOnly = BuildUtils.IsReadOnly();
        string readOnlyWarning = readOnly ? "\n\nWARNING: Build Settings is not checked out and so cannot be modified." : "";

        GUIContent iconContent = new GUIContent();
        GUIContent labelContent = new GUIContent();
        
        if (buildScene.buildIndex == -1)
        {
            // Missing from build scenes.
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_close");
            labelContent.text = "NOT In Build";
            labelContent.tooltip = "This scene is NOT in build settings.\nIt will be NOT included in builds.";
        }
        else if (buildScene.scene.enabled)
        {
            // In build scenes and enabled.
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_max");
            labelContent.text = "BuildIndex: " + buildScene.buildIndex;
            labelContent.tooltip = "This scene is in build settings and ENABLED.\nIt will be included in builds." + readOnlyWarning;
        }
        else
        {
            // In build scenes and disabled.
            iconContent = EditorGUIUtility.IconContent("d_winbtn_mac_min");
            labelContent.text = "BuildIndex: " + buildScene.buildIndex;
            labelContent.tooltip = "This scene is in build settings and DISABLED.\nIt will be NOT included in builds.";
        }

        // Left status label.
        using (new EditorGUI.DisabledScope(readOnly))
        {
            var labelRect = DrawUtils.GetLabelRect(position);
            var iconRect = labelRect;
            iconRect.width = iconContent.image.width + PAD_SIZE;
            labelRect.width -= iconRect.width;
            labelRect.x += iconRect.width;
            EditorGUI.PrefixLabel(iconRect, sceneControlID, iconContent);
            EditorGUI.PrefixLabel(labelRect, sceneControlID, labelContent);
        }

        // Right context buttons.
        var buttonRect = DrawUtils.GetFieldRect(position);
        buttonRect.width = (buttonRect.width) / 3;

        var tooltipMsg = "";
        using (new EditorGUI.DisabledScope(readOnly))
        {
            if (buildScene.buildIndex == -1)
            {
                // NOT in build settings.
                buttonRect.width *= 2;
                int addIndex = EditorBuildSettings.scenes.Length;
                tooltipMsg = "Add this scene to build settings. It will be appended to the end of the build scenes as buildIndex: " + addIndex + "." + readOnlyWarning;
                if (DrawUtils.ButtonHelper(buttonRect, "Add...", "Add (buildIndex " + addIndex + ")", EditorStyles.miniButtonLeft, tooltipMsg))
                    BuildUtils.AddBuildScene(buildScene);
                buttonRect.width /= 2;
                buttonRect.x += buttonRect.width;
            }
            else
            {
                // IS in build settings.
                bool isEnabled = buildScene.scene.enabled;
                string stateString = isEnabled ? "Disable" : "Enable";
                tooltipMsg = stateString + " this scene in build settings.\n" + (isEnabled ? "It will no longer be included in builds" : "It will be included in builds") + "." + readOnlyWarning;

                if (DrawUtils.ButtonHelper(buttonRect, stateString, stateString + " In Build", EditorStyles.miniButtonLeft, tooltipMsg))
                    BuildUtils.SetBuildSceneState(buildScene, !isEnabled);
                buttonRect.x += buttonRect.width;

                tooltipMsg = "Completely remove this scene from build settings.\nYou will need to add it again for it to be included in builds!" + readOnlyWarning;
                if (DrawUtils.ButtonHelper(buttonRect, "Remove...", "Remove from Build", EditorStyles.miniButtonMid, tooltipMsg))
                    BuildUtils.RemoveBuildScene(buildScene);
            }
        }

        buttonRect.x += buttonRect.width;

        tooltipMsg = "Open the 'Build Settings' Window for managing scenes." + readOnlyWarning;
        if (DrawUtils.ButtonHelper(buttonRect, "Settings", "Build Settings", EditorStyles.miniButtonRight, tooltipMsg))
            BuildUtils.OpenBuildSettings();
    }

    private static SerializedProperty GetSceneAssetProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(SCENE_ASSET_PROPERTY_STRING);
    }

    private static SerializedProperty GetScenePathProperty(SerializedProperty property)
    {
        return property.FindPropertyRelative(SCENE_PATH_PROPERTY_STRING);
    }

    private static class DrawUtils
    {
        /// <summary>
        /// Draw a GUI button, choosing between a short and a long button text based on if it fits.
        /// </summary>
        public static bool ButtonHelper(Rect position, string msgShort, string msgLong, GUIStyle style, string tooltip = null)
        {
            var content = new GUIContent(msgLong) { tooltip = tooltip };

            var longWidth = style.CalcSize(content).x;
            if (longWidth > position.width)
                content.text = msgShort;

            return GUI.Button(position, content, style);
        }

        /// <summary>
        /// Given a position rect, get its field portion.
        /// </summary>
        public static Rect GetFieldRect(Rect position)
        {
            position.width -= EditorGUIUtility.labelWidth;
            position.x += EditorGUIUtility.labelWidth;
            return position;
        }

        /// <summary>
        /// Given a position rect, get its label portion.
        /// </summary>
        public static Rect GetLabelRect(Rect position)
        {
            position.width = EditorGUIUtility.labelWidth - PAD_SIZE;
            return position;
        }
    }

    private static class BuildUtils
    {
        // Time in seconds that we have to wait before we query again when IsReadOnly() is called.
        public static float MinCheckWait = 3;

        private static float _lastTimeChecked = 0.0f;
        private static bool _cachedReadonlyVal = true;

        /// <summary>
        /// A small container for tracking scene data BuildSettings
        /// </summary>
        public struct BuildScene
        {
            public int buildIndex;
            public GUID assetGUID;
            public string assetPath;
            public EditorBuildSettingsScene scene;
        }

        /// <summary>
        /// Check if the build settings asset is readonly.
        /// Caches value and only queries state a max of every 'minCheckWait' seconds.
        /// </summary>
        public static bool IsReadOnly()
        {
            float curTime = Time.realtimeSinceStartup;
            float timeSinceLastCheck = curTime - _lastTimeChecked;

            if (!(timeSinceLastCheck > MinCheckWait))
                return _cachedReadonlyVal;

            _lastTimeChecked = curTime;
            _cachedReadonlyVal = QueryBuildSettingsStatus();

            return _cachedReadonlyVal;
        }

        /// <summary>
        /// A blocking call to the Version Control system to see if the build settings asset is readonly.
        /// Use BuildSettingsIsReadOnly for version that caches the value for better responsivenes.
        /// </summary>
        private static bool QueryBuildSettingsStatus()
        {
            // If there are no version control provider, assume it's not readonly.
            if (!Provider.enabled)
                return false;

            // If we cannot checkout, then assume we are not readonly.
            if (!Provider.hasCheckoutSupport)
                return false;

            //// If offline (and are using a version control provider that requires checkout) we cannot edit.
            if (UnityEditor.VersionControl.Provider.onlineState == UnityEditor.VersionControl.OnlineState.Offline)
                return true;

            // Try to get the file's status.
            Task status = Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
            status.Wait();

            // If there are no status listed, we can edit.
            if (status.assetList == null || status.assetList.Count != 1)
                return true;

            // If it is checked out, we can edit.
            return !status.assetList[0].IsState(Asset.States.CheckedOutLocal);
        }

        /// <summary>
        /// For a given Scene Asset object reference, extract its build settings data, including buildIndex.
        /// </summary>
        public static BuildScene GetBuildScene(Object sceneObject)
        {
            BuildScene entry = new BuildScene
            {
                buildIndex = -1,
                assetGUID = new GUID(string.Empty)
            };

            if (sceneObject as SceneAsset == null)
                return entry;

            entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
            entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; ++i)
            {
                if (!entry.assetGUID.Equals(scenes[i].guid))
                    continue;

                entry.scene = scenes[i];
                entry.buildIndex = i;
                return entry;
            }

            return entry;
        }

        /// <summary>
        /// Enable/Disable a given scene in the buildSettings.
        /// </summary>
        public static void SetBuildSceneState(BuildScene buildScene, bool enabled)
        {
            var modified = false;
            var scenesToModify = EditorBuildSettings.scenes;
            foreach (var curScene in scenesToModify.Where(curScene => curScene.guid.Equals(buildScene.assetGUID)))
            {
                curScene.enabled = enabled;
                modified = true;
                break;
            }
            if (modified) EditorBuildSettings.scenes = scenesToModify;
        }

        /// <summary>
        /// Display Dialog to add a scene to build settings
        /// </summary>
        public static void AddBuildScene(BuildScene buildScene, bool force = false, bool enabled = true)
        {
            if (force == false)
            {
                var selection = EditorUtility.DisplayDialogComplex(
                    "Add Scene To Build",
                    "You are about to add scene at " + buildScene.assetPath + " To the Build Settings.",
                    "Add as Enabled",       // option 0
                    "Add as Disabled",      // option 1
                    "Cancel (do nothing)"); // option default

                switch (selection)
                {
                    case 0:
                        enabled = true;
                        break;
                    case 1:
                        enabled = false;
                        break;
                    default:
                        return;
                }
            }

            var newScene = new EditorBuildSettingsScene(buildScene.assetGUID, enabled);
            var tempScenes = EditorBuildSettings.scenes.ToList();
            tempScenes.Add(newScene);
            EditorBuildSettings.scenes = tempScenes.ToArray();
        }

        /// <summary>
        /// Display Dialog to remove a scene from build settings (or just disable it)
        /// </summary>
        public static void RemoveBuildScene(BuildScene buildScene, bool force = false)
        {
            var onlyDisable = false;
            if (force == false)
            {
                int selection = -1;

                string title = "Remove Scene From Build";
                string details = $"You are about to remove the following scene from build settings:\n    {buildScene.assetPath}\n    buildIndex: {buildScene.buildIndex}\n\nThis will modify build settings, but the scene asset will remain untouched.";
                string confirm = "Remove From Build";
                string alt = "Just Disable";
                string cancel = "Cancel (do nothing)";

                if (buildScene.scene.enabled)
                {
                    details += "\n\nIf you want, you can also just disable it instead.";
                    selection = EditorUtility.DisplayDialogComplex(title, details, confirm, alt, cancel);
                }
                else
                {
                    selection = EditorUtility.DisplayDialog(title, details, confirm, cancel) ? 0 : 2;
                }

                switch (selection)
                {
                    case 0: // Remove
                        break;
                    case 1: // Disable
                        onlyDisable = true;
                        break;
                    default: // Cancel
                        return;
                }
            }
            
            if (onlyDisable)
            {
                // User chose not to remove, only disabling the scene.
                SetBuildSceneState(buildScene, false);
            }
            else
            {
                // User chose to fully remove the scene from build settings
                List<EditorBuildSettingsScene> tempScenes = EditorBuildSettings.scenes.ToList();
                tempScenes.RemoveAll(scene => scene.guid.Equals(buildScene.assetGUID));
                EditorBuildSettings.scenes = tempScenes.ToArray();
            }
        }

        /// <summary>
        /// Open the default Unity Build Settings window.
        /// </summary>
        public static void OpenBuildSettings()
        {
            EditorWindow.GetWindow(typeof(BuildPlayerWindow));
        }
    }
}

#endif