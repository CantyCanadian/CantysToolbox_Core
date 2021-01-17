// ====================================================================================================
//
// Foldout Attribute
//
// Original Code by Dmitry Mitrofanov [https://github.com/PixeyeHQ]
// Edited by Andrew Rumak [https://github.com/Deadcows] and Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

namespace Canty
{
    public class FoldoutAttribute : PropertyAttribute
    {
        public readonly string Name;
        public readonly bool FoldEverything;

        /// <summary>Adds the property to the specified foldout group.</summary>
        /// <param name="name">Name of the foldout group.</param>
        /// <param name="foldEverything">Toggle to put all properties to the specified group</param>
        public FoldoutAttribute(string name, bool foldEverything = false)
        {
            FoldEverything = foldEverything;
            Name = name;
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;

    public class FoldoutAttributeHandler
    {
        public bool OverrideInspector { get { return m_Props.Count > 0; } }

        private Dictionary<string, CacheFoldProp> m_CacheFolds = new Dictionary<string, CacheFoldProp>();
        private List<SerializedProperty> m_Props = new List<SerializedProperty>();
        private bool m_Initialized;

        private UnityEngine.Object m_Target;
        private SerializedObject m_SerializedObject;

        public FoldoutAttributeHandler(UnityEngine.Object target, SerializedObject serializedObject)
        {
            m_Target = target;
            m_SerializedObject = serializedObject;
        }

        public void OnDisable()
        {
            if (m_Target == null)
            {
                return;
            }

            foreach (KeyValuePair<string, CacheFoldProp> cache in m_CacheFolds)
            {
                EditorPrefs.SetBool(string.Format($"{cache.Value.atr.Name}{cache.Value.props[0].name}{m_Target.name}"), cache.Value.expanded);
                cache.Value.Dispose();
            }
        }

        public void Update()
        {
            m_SerializedObject.Update();
            Setup();
        }

        public void OnInspectorGUI()
        {
            Header();
            Body();

            m_SerializedObject.ApplyModifiedProperties();
        }

        private void Header()
        {
            using (new EditorGUI.DisabledScope("m_Script" == m_Props[0].propertyPath))
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_Props[0], true);
                EditorGUILayout.Space();
            }
        }

        private void Body()
        {
            foreach (KeyValuePair<string, CacheFoldProp> cache in m_CacheFolds)
            {
                EditorGUILayout.BeginVertical(StyleFramework.Box);
                Foldout(cache.Value);
                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel = 0;
            }

            EditorGUILayout.Space();

            for (int i = 1; i < m_Props.Count; i++)
            {
                EditorGUILayout.PropertyField(m_Props[i], true);
            }

            EditorGUILayout.Space();
        }

        private void Foldout(CacheFoldProp cache)
        {
            cache.expanded = EditorGUILayout.Foldout(cache.expanded, cache.atr.Name, true, StyleFramework.Foldout);

            if (cache.expanded)
            {
                EditorGUI.indentLevel = 1;

                for (int i = 0; i < cache.props.Count; i++)
                {
                    EditorGUILayout.BeginVertical(StyleFramework.BoxChild);
                    EditorGUILayout.PropertyField(cache.props[i], new GUIContent(cache.props[i].name.FirstLetterToUpperCase()), true);
                    EditorGUILayout.EndVertical();
                }
            }
        }

        private void Setup()
        {
            if (m_Initialized)
            {
                return;
            }

            List<FieldInfo> objectFields;
            FoldoutAttribute prevFold = default;

            int length = EditorTypes.Get(m_Target, out objectFields);

            for (int i = 0; i < length; i++)
            {
                FoldoutAttribute fold = Attribute.GetCustomAttribute(objectFields[i], typeof(FoldoutAttribute)) as FoldoutAttribute;
                CacheFoldProp c;
                if (fold == null)
                {
                    if (prevFold != null && prevFold.FoldEverything)
                    {
                        if (!m_CacheFolds.TryGetValue(prevFold.Name, out c))
                        {
                            m_CacheFolds.Add(prevFold.Name, new CacheFoldProp { atr = prevFold, types = new HashSet<string> { objectFields[i].Name } });
                        }
                        else
                        {
                            c.types.Add(objectFields[i].Name);
                        }
                    }

                    continue;
                }

                prevFold = fold;

                if (!m_CacheFolds.TryGetValue(fold.Name, out c))
                {
                    bool expanded = EditorPrefs.GetBool(string.Format($"{fold.Name}{objectFields[i].Name}{m_Target.name}"), false);
                    m_CacheFolds.Add(fold.Name, new CacheFoldProp { atr = fold, types = new HashSet<string> { objectFields[i].Name }, expanded = expanded });
                }
                else
                {
                    c.types.Add(objectFields[i].Name);
                }
            }

            SerializedProperty property = m_SerializedObject.GetIterator();
            bool next = property.NextVisible(true);
            if (next)
            {
                do
                {
                    HandleFoldProp(property);
                }
                while (property.NextVisible(false));
            }

            m_Initialized = true;
        }

        private void HandleFoldProp(SerializedProperty prop)
        {
            bool shouldBeFolded = false;

            foreach (KeyValuePair<string, CacheFoldProp> cache in m_CacheFolds)
            {
                if (cache.Value.types.Contains(prop.name))
                {
                    var pr = prop.Copy();
                    shouldBeFolded = true;
                    cache.Value.props.Add(pr);

                    break;
                }
            }

            if (shouldBeFolded == false)
            {
                var pr = prop.Copy();
                m_Props.Add(pr);
            }
        }

        class CacheFoldProp
        {
            public HashSet<string> types = new HashSet<string>();
            public List<SerializedProperty> props = new List<SerializedProperty>();
            public FoldoutAttribute atr;
            public bool expanded;

            public void Dispose()
            {
                props.Clear();
                types.Clear();
                atr = null;
            }
        }
    }
    
    static class StyleFramework
    {
        public static readonly GUIStyle Box;
        public static readonly GUIStyle BoxChild;
        public static readonly GUIStyle Foldout;

        static StyleFramework()
        {
            bool pro = EditorGUIUtility.isProSkin;

            Texture2D uiTexIn = Resources.Load<Texture2D>("IN foldout focus-6510");
            Texture2D uiTexInOn = Resources.Load<Texture2D>("IN foldout focus on-5718");

            Color colorOn = pro ? Color.white : new Color(51 / 255f, 102 / 255f, 204 / 255f, 1);

            Foldout = new GUIStyle(EditorStyles.foldout);

            Foldout.overflow = new RectOffset(-10, 0, 3, 0);
            Foldout.padding = new RectOffset(25, 0, -3, 0);

            Foldout.active.textColor = colorOn;
            Foldout.active.background = uiTexIn;
            Foldout.onActive.textColor = colorOn;
            Foldout.onActive.background = uiTexInOn;

            Foldout.focused.textColor = colorOn;
            Foldout.focused.background = uiTexIn;
            Foldout.onFocused.textColor = colorOn;
            Foldout.onFocused.background = uiTexInOn;

            Foldout.hover.textColor = colorOn;
            Foldout.hover.background = uiTexIn;

            Foldout.onHover.textColor = colorOn;
            Foldout.onHover.background = uiTexInOn;

            Box = new GUIStyle(GUI.skin.box);
            Box.padding = new RectOffset(10, 0, 10, 0);

            BoxChild = new GUIStyle(GUI.skin.box);
            BoxChild.active.textColor = colorOn;
            BoxChild.active.background = uiTexIn;
            BoxChild.onActive.textColor = colorOn;
            BoxChild.onActive.background = uiTexInOn;

            BoxChild.focused.textColor = colorOn;
            BoxChild.focused.background = uiTexIn;
            BoxChild.onFocused.textColor = colorOn;
            BoxChild.onFocused.background = uiTexInOn;

            EditorStyles.foldout.active.textColor = colorOn;
            EditorStyles.foldout.active.background = uiTexIn;
            EditorStyles.foldout.onActive.textColor = colorOn;
            EditorStyles.foldout.onActive.background = uiTexInOn;

            EditorStyles.foldout.focused.textColor = colorOn;
            EditorStyles.foldout.focused.background = uiTexIn;
            EditorStyles.foldout.onFocused.textColor = colorOn;
            EditorStyles.foldout.onFocused.background = uiTexInOn;

            EditorStyles.foldout.hover.textColor = colorOn;
            EditorStyles.foldout.hover.background = uiTexIn;

            EditorStyles.foldout.onHover.textColor = colorOn;
            EditorStyles.foldout.onHover.background = uiTexInOn;
        }

        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static IList<Type> GetTypeTree(this Type t)
        {
            List<Type> types = new List<Type>();
            while (t.BaseType != null)
            {
                types.Add(t);
                t = t.BaseType;
            }

            return types;
        }
    }

    static class EditorTypes
    {
        public static Dictionary<int, List<FieldInfo>> fields = new Dictionary<int, List<FieldInfo>>(FastComparable.Default);

        public static int Get(Object target, out List<FieldInfo> objectFields)
        {
            Type t = target.GetType();
            int hash = t.GetHashCode();

            if (!fields.TryGetValue(hash, out objectFields))
            {
                IList<Type> typeTree = t.GetTypeTree();
                objectFields = target.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.NonPublic)
                    .OrderByDescending(x => typeTree.IndexOf(x.DeclaringType))
                    .ToList();
                fields.Add(hash, objectFields);
            }

            return objectFields.Count;
        }
    }


    class FastComparable : IEqualityComparer<int>
    {
        public static FastComparable Default = new FastComparable();

        public bool Equals(int x, int y)
        {
            return x == y;
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }
    }
}
#endif