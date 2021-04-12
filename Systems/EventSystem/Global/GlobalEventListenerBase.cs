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
    public abstract class GlobalEventListenerBase<EventBaseType> : EventListenerBase
        where EventBaseType : EventBase
    {
        protected GlobalEventDispatcherBase<EventBaseType> _dispatcher = null;

        protected virtual void Awake()
        {
            _dispatcher = GlobalEventDispatcherBase<EventBaseType>.RegisterListener(this);
            Debug.Assert(_dispatcher != null);
        }
    }    
}