using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Canty.EventSystem
{
    public class EventDispatcherObject<EventListenerType, EventBaseType>
        where EventListenerType : EventListenerBase
        where EventBaseType : EventBase
    {
        private Dictionary<Type, List<(EventListenerBase Listener, MethodInfo Method)>> m_Listeners = new Dictionary<Type, List<(EventListenerBase listener, MethodInfo method)>>();
        private Queue<(EventBaseType Event, Type EventType)> m_EventQueue = new Queue<(EventBaseType Event, Type EventType)>();

        /// <summary>
        /// Since we can assure that events on the other doesn't change as they are sent, we're caching them here to prevent any mid-sending changes.
        /// </summary>
        private Dictionary<Type, EventBaseType> m_EventCache = new Dictionary<Type, EventBaseType>();

        private bool m_IsProcessing = false;

        public int RegisterEventListener(EventListenerBase listener)
        {
            var methods = listener.GetType().GetMethods()
                .Select(method => (Method: method, Attribute: method.GetCustomAttribute<EventReceiverAttribute>(), Params: method.GetParameters()))
                .Where(methodTuple => (methodTuple.Method.Name == "ReceiveEvent" || methodTuple.Attribute != null) && methodTuple.Params.Length == 1)
                .Select(methodTuple => (Method: methodTuple.Method, Type: methodTuple.Params[0].ParameterType));

            int methodCount = 0;
            foreach (var method in methods)
            {
                if (method.Type == typeof(EventBaseType))
                {
                    if (!m_Listeners.TryGetValue(typeof(EventBaseType), out var values))
                    {
                        m_Listeners.Add(typeof(EventBaseType), values = new List<(EventListenerBase Listener, MethodInfo Method)>());
                    }

                    values.Add((listener, method.Method));
                    methodCount++;
                }
                else if (method.Type.IsSubclassOf(typeof(EventBaseType)))
                {
                    if (!m_Listeners.TryGetValue(method.Type, out var values))
                    {
                        m_Listeners.Add(method.Type, values = new List<(EventListenerBase Listener, MethodInfo Method)>());
                    }

                    values.Add((listener, method.Method));
                    methodCount++;
                }
            }

            return methodCount;
        }

        public void SendEvent<EventType>(EventType eventObject) where EventType : EventBaseType
        {
            Type type = typeof(EventType);

            if (!m_EventCache.ContainsKey(type))
                m_EventCache.Add(type, Activator.CreateInstance<EventType>());

            m_EventQueue.Enqueue((eventObject, typeof(EventType)));

            if (!m_IsProcessing)
            {
                m_IsProcessing = true;

                while (m_EventQueue.Count > 0)
                {
                    var currentEvent = m_EventQueue.Dequeue();

                    // We're caching the latest of each events to prevent issues where an event object could be changed mid-way.
                    m_EventCache[currentEvent.EventType].Copy(currentEvent.Event);
                    currentEvent.Event = m_EventCache[currentEvent.EventType];

                    if (m_Listeners.TryGetValue(typeof(EventBaseType), out var genericMethods))
                    {
                        var eventParameter = new object[] { currentEvent.Event };

                        foreach (var method in genericMethods)
                        {
                            method.Method.Invoke(method.Listener, eventParameter);
                        }
                    }

                    if (m_Listeners.TryGetValue(currentEvent.EventType, out var specificMethods))
                    {
                        var eventParameter = new object[] { Convert.ChangeType(currentEvent.Event, currentEvent.EventType) };

                        foreach (var method in specificMethods)
                        {
                            method.Method.Invoke(method.Listener, eventParameter);
                        }
                    }
                }

                m_IsProcessing = false;
            }
        }
    }
}