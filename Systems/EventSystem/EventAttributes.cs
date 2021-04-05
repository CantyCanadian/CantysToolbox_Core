using System;

namespace Canty.EventSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventReceiverAttribute : Attribute
    {
        public EventReceiverAttribute() { }
    }
}
