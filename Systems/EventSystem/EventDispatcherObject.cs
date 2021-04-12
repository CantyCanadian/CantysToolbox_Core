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
        private Dictionary<Type, List<(EventListenerBase Listener, MethodInfo Method)>> _listeners = new Dictionary<Type, List<(EventListenerBase listener, MethodInfo method)>>();
        private Queue<(EventBaseType Event, Type EventType)> _eventQueue = new Queue<(EventBaseType Event, Type EventType)>();

        /// <summary>
        /// Since we can assure that events on the other doesn't change as they are sent, we're caching them here to prevent any mid-sending changes.
        /// </summary>
        private Dictionary<Type, EventBaseType> _eventCache = new Dictionary<Type, EventBaseType>();

        private bool _isProcessing = false;

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
                    if (!_listeners.TryGetValue(typeof(EventBaseType), out var values))
                    {
                        _listeners.Add(typeof(EventBaseType), values = new List<(EventListenerBase Listener, MethodInfo Method)>());
                    }

                    values.Add((listener, method.Method));

                    foreach(var eventObject in _eventCache)
                    {
                        if (!eventObject.Value.GetIsInstant())
                            method.Method.Invoke(listener, new object[] { eventObject });
                    }

                    methodCount++;
                }
                else if (method.Type.IsSubclassOf(typeof(EventBaseType)))
                {
                    if (!_listeners.TryGetValue(method.Type, out var values))
                    {
                        _listeners.Add(method.Type, values = new List<(EventListenerBase Listener, MethodInfo Method)>());
                    }

                    values.Add((listener, method.Method));

                    foreach (var eventObject in _eventCache)
                    {
                        if (eventObject.Key == method.Type && !eventObject.Value.GetIsInstant())
                            method.Method.Invoke(listener, new object[] { Convert.ChangeType(eventObject.Value, eventObject.Key) });
                    }

                    methodCount++;
                }
            }

            return methodCount;
        }

        public void SendEvent<EventType>(EventType eventObject) where EventType : EventBaseType
        {
            Type type = typeof(EventType);

            if (!_eventCache.ContainsKey(type))
                _eventCache.Add(type, Activator.CreateInstance<EventType>());

            _eventQueue.Enqueue((eventObject, typeof(EventType)));

            if (!_isProcessing)
            {
                _isProcessing = true;

                while (_eventQueue.Count > 0)
                {
                    var currentEvent = _eventQueue.Dequeue();

                    // We're caching the latest of each events to prevent issues where an event object could be changed mid-way.
                    _eventCache[currentEvent.EventType].Copy(currentEvent.Event);
                    currentEvent.Event = _eventCache[currentEvent.EventType];

                    if (_listeners.TryGetValue(typeof(EventBaseType), out var genericMethods))
                    {
                        var eventParameter = new object[] { currentEvent.Event };

                        foreach (var method in genericMethods)
                        {
                            method.Method.Invoke(method.Listener, eventParameter);
                        }
                    }

                    if (_listeners.TryGetValue(currentEvent.EventType, out var specificMethods))
                    {
                        var eventParameter = new object[] { Convert.ChangeType(currentEvent.Event, currentEvent.EventType) };

                        foreach (var method in specificMethods)
                        {
                            method.Method.Invoke(method.Listener, eventParameter);
                        }
                    }
                }

                _isProcessing = false;
            }
        }
    }
}