#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using System;
using UnityEditor.Build.Reporting;

namespace Canty
{
    /// <summary>
    /// Collection of editor-only static events you can link your editor tools / attributes to.
    /// </summary>
    public class EditorEvents : UnityEditor.AssetModificationProcessor, IPreprocessBuildWithReport
    {
        /// <summary>
		/// Occurs on Scenes/Assets Save
		/// </summary>
		public static Action OnSave;

        /// <summary>
		/// Occurs on first frame in Playmode
		/// </summary>
		public static Action OnFirstFrame;

        public static Action BeforePlaymode;

        public static Action BeforeBuild;

        // Necessary call for interface.
        public int callbackOrder { get { return 0; } }

        static EditorEvents()
        {
            EditorApplication.update += CheckOnce;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// On Editor Save
        /// </summary>
        private static string[] OnWillSaveAssets(string[] paths)
        {
            // Prefab creation enforces SaveAsset and we don't want to trigger the event in that case.
            if (paths.Length == 1 && (paths[0] == null || paths[0].EndsWith(".prefab")))
            {
                return paths;
            }

            if (OnSave != null)
            {
                OnSave();
            }

            return paths;
        }

        private static void CheckOnce()
        {
            if (Application.isPlaying)
            {
                EditorApplication.update -= CheckOnce;
                OnFirstFrame?.Invoke();
            }
        }

        /// <summary>
		/// On Before Playmode
		/// </summary>
		private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode && BeforePlaymode != null)
            {
                BeforePlaymode();
            }
        }

        /// <summary>
		/// Before Build
		/// </summary>
		public void OnPreprocessBuild(BuildReport report)
        {
            BeforeBuild?.Invoke();
        }
    }
}

#endif