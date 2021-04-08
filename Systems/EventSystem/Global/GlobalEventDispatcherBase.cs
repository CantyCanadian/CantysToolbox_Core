using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Canty.EventSystem
{
    [DefaultExecutionOrder(-10)]
    public abstract class GlobalEventDispatcherBase<EventBaseType> : GlobalEventDispatcherBase
        where EventBaseType : EventBase
    {
        [SerializeField] private bool m_ShowDebugLogs = false;

        public static GlobalEventDispatcherBase<EventBaseType> GetDispatcher()
        {
            bool success = m_Dispatchers.TryGetValue(typeof(EventBaseType), out GlobalEventDispatcherBase dispatcher);
            Debug.Assert(success, $"No dispatcher using event type [{typeof(EventBaseType)}] to register listener to.");

            var eventDispatcher = dispatcher as GlobalEventDispatcherBase<EventBaseType>;

            Debug.Assert(eventDispatcher != null, $"No dispatcher of the right type using event type [{typeof(EventBaseType)}] to register listener to.");

            return eventDispatcher;
        }

        public static GlobalEventDispatcherBase<EventBaseType> RegisterListener(GlobalEventListenerBase<EventBaseType> listener)
        {
            var dispatcher = GetDispatcher();
            dispatcher.RegisterEventListener(listener);
            return dispatcher;
        }

        protected EventDispatcherObject<GlobalEventListenerBase<EventBaseType>, EventBaseType> m_DispatcherObject = new EventDispatcherObject<GlobalEventListenerBase<EventBaseType>, EventBaseType>();

        public void RegisterEventListener(GlobalEventListenerBase<EventBaseType> listener)
        {
            int methodCount = m_DispatcherObject.RegisterEventListener(listener);

            if (m_ShowDebugLogs && methodCount > 0)
                Debug.Log($"[{GetType().Name}] : Registered listener [{listener.name}], methods registered [{methodCount}].");
        }

        public void SendEvent<EventType>(EventType eventObject) where EventType : EventBaseType
        {
            m_DispatcherObject.SendEvent(eventObject);

            if (m_ShowDebugLogs)
                Debug.Log($"{eventObject.GetDebugData()}");
        }

        private void Awake()
        {
            if (!m_Dispatchers.ContainsKey(GetType()))
            {
                m_Dispatchers.Add(typeof(EventBaseType), this);
            }
        }
    }

    public abstract class GlobalEventDispatcherBase : MonoBehaviour
    {
        protected static Dictionary<Type, GlobalEventDispatcherBase> m_Dispatchers = new Dictionary<Type, GlobalEventDispatcherBase>();
    }
}