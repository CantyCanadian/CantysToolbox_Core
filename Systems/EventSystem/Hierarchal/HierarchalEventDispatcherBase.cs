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
                Debug.Log($"<color=lime>[{GetType().Name}]</color>\nRegistered listener [{listener.GetType().Name}], methods registered [{methodCount}].");
        }

        public void SendEvent<EventType>(EventType eventObject) where EventType : EventBaseType
        {
            if (_showDebugLogs)
                Debug.Log($"<color=magenta>[{eventObject.Origin}]</color>\n{eventObject.GetDebugData()}");

            _dispatcherObject.SendEvent(eventObject);
        }
    }
}