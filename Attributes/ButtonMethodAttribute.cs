// ====================================================================================================
//
// Button Method Attribute
//
// Original Code by Kaynn, Yeo Wen Qin [https://github.com/Kaynn-Cahya]
// Edited by Andrew Rumak [https://github.com/Deadcows] and Anthony Duquette [https://github.com/CantyCanadian]
//
// ====================================================================================================

using UnityEngine;

namespace Canty
{
    public class ButtonMethodAttribute : PropertyAttribute
    {
        public readonly ButtonMethodDrawOrder DrawOrder;

        /// <summary>
        /// Adding this attribute to a method will make it appear as a button in the inspector. Returning a string in the attributed method will output the result as a log.
        /// </summary>
        /// <param name="drawOrder">Where should the button be added in the inspector.</param>
        public ButtonMethodAttribute(ButtonMethodDrawOrder drawOrder = ButtonMethodDrawOrder.AfterInspector)
        {
            DrawOrder = drawOrder;
        }
        
        public enum ButtonMethodDrawOrder
        {
            BeforeInspector,
            AfterInspector
        }
    }
}

#if UNITY_EDITOR
namespace Canty.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;

    public class ButtonMethodHandler
    {
        public readonly List<(MethodInfo Method, string Name, ButtonMethodAttribute.ButtonMethodDrawOrder Order)> TargetMethods;

        public int Amount { get { return TargetMethods?.Count ?? 0; } }

        private readonly UnityEngine.Object m_Target;

        public ButtonMethodHandler(UnityEngine.Object target)
        {
            m_Target = target;

            Type type = target.GetType();
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            IEnumerable<MemberInfo> members = type.GetMembers(bindings).Where(IsButtonMethod);

            foreach (MemberInfo member in members)
            {
                MethodInfo method = member as MethodInfo;

                if (method == null)
                {
                    continue;
                }

                if (IsValidMember(method, member))
                {
                    ButtonMethodAttribute attribute = Attribute.GetCustomAttribute(method, typeof(ButtonMethodAttribute)) as ButtonMethodAttribute;

                    if (TargetMethods == null)
                    {
                        TargetMethods = new List<(MethodInfo, string, ButtonMethodAttribute.ButtonMethodDrawOrder)>();
                    }

                    TargetMethods.Add((method, method.Name.SpaceCamelCase(), attribute.DrawOrder));
                }
            }
        }

        public void OnBeforeInspectorGUI()
        {
            if (TargetMethods == null)
            {
                return;
            }

            foreach ((MethodInfo Method, string Name, ButtonMethodAttribute.ButtonMethodDrawOrder Order) in TargetMethods)
            {
                if (Order != ButtonMethodAttribute.ButtonMethodDrawOrder.BeforeInspector)
                {
                    continue;
                }

                if (GUILayout.Button(Name))
                {
                    InvokeMethod(m_Target, Method);
                }
            }

            EditorGUILayout.Space();
        }

        public void OnAfterInspectorGUI()
        {
            if (TargetMethods == null)
            {
                return;
            }

            EditorGUILayout.Space();

            foreach ((MethodInfo Method, string Name, ButtonMethodAttribute.ButtonMethodDrawOrder Order) in TargetMethods)
            {
                if (Order != ButtonMethodAttribute.ButtonMethodDrawOrder.AfterInspector)
                {
                    continue;
                }

                if (GUILayout.Button(Name))
                {
                    InvokeMethod(m_Target, Method);
                }
            }
        }

        public void Invoke(MethodInfo method)
        {
            InvokeMethod(m_Target, method);
        }

        private void InvokeMethod(UnityEngine.Object target, MethodInfo method)
        {
            object result = method.Invoke(target, null);

            if (result != null)
            {
                string message = $"{result} \nResult of Method [{method.Name}] invocation on object [{target.name}].";
                Debug.Log(message, target);
            }
        }

        private bool IsButtonMethod(MemberInfo memberInfo)
        {
            return Attribute.IsDefined(memberInfo, typeof(ButtonMethodAttribute));
        }

        private bool IsValidMember(MethodInfo method, MemberInfo member)
        {
            if (method == null)
            {
                Debug.LogError($"Property [{member.Name}] is not a method but has EditorButtonAttribute.");
                return false;
            }

            if (method.GetParameters().Length > 0)
            {
                Debug.LogError($"Method [{method.Name}] is a method with parameters which aren't supported by EditorButtonAttribute.");
                return false;
            }

            return true;
        }
    }
}
#endif