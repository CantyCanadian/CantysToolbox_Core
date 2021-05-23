using UnityEngine;

namespace Canty.EventSystem
{
    public class HierarchalEventDispatcherBase<EventBaseType> : MonoBehaviour
        where EventBaseType : EventBase
    {
        [SerializeField] protected bool _showDebugLogs = false;

        protected EventDispatcherObject<HierarchalEventListenerBase<EventBaseType>, EventBaseType> _dispatcherObject = new EventDispatcherObject<HierarchalEventListenerBase<EventBaseType>, EventBaseType>();

        public void RegisterEventListener(HierarchalEventListenerBase<EventBaseType> listener)
        {
            int methodCount = _dispatcherObject.RegisterEventListener(listener);

            if (_showDebugLogs && methodCount > 0)
                Debug.Log($"[{GetType().Name}] : Registered listener [{listener.name}], methods registered [{methodCount}].");
        }

        public void SendEvent<EventType>(EventType eventObject) where EventType : EventBaseType
        {
            _dispatcherObject.SendEvent(eventObject);

            if (_showDebugLogs)
                Debug.Log($"{eventObject.GetDebugData()}");
        }
    }
}