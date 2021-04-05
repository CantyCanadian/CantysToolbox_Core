using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Canty.EventSystem
{
    /// <summary>
    /// In order for a class to receive events from a global listener, they must inherit from this class.
    /// Difference between a global and hierarchal listener? Global uses a singleton dispatcher, hierarchal assumes the dispatcher is at the root of your hierarchy.
    /// Any method with one parameter and the attribute [EventReceiver] or with the name "ReceiveEvent" will receive events that comes from the dispatcher.
    /// What event gets received depends on the first parameter of your function. Can be either EventBaseType (receives all events) or a specific event type.
    /// </summary>
    /// <typeparam name="EventBaseType">EventBase type this listener should expect. It is heavily recommended to create a new type that inherits from the desired EventBase.</typeparam>
    public abstract class HierarchalEventListenerBase<EventBaseType> : EventListenerBase
        where EventBaseType : EventBase
    {
        protected HierarchalEventDispatcherBase<EventBaseType> m_Dispatcher = null;

        protected virtual void Awake()
        {
            m_Dispatcher = GetComponentInParent<HierarchalEventDispatcherBase<EventBaseType>>();
            Debug.Assert(m_Dispatcher != null);

            m_Dispatcher.RegisterEventListener(this);
        }
    }
}