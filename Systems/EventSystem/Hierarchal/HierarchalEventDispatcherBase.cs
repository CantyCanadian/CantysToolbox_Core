using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Canty.EventSystem
{
    public class HierarchalEventDispatcherBase<EventBaseType> : MonoBehaviour
        where EventBaseType : EventBase
    {
        protected EventDispatcherObject<HierarchalEventListenerBase<EventBaseType>, EventBaseType> _dispatcherObject = new EventDispatcherObject<HierarchalEventListenerBase<EventBaseType>, EventBaseType>();

        public void RegisterEventListener(HierarchalEventListenerBase<EventBaseType> listener)
        {
            _dispatcherObject.RegisterEventListener(listener);
        }

        public void SendEvent<EventType>(EventType eventObject) where EventType : EventBaseType
        {
            _dispatcherObject.SendEvent(eventObject);
        }
    }
}