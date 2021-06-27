namespace Canty.EventSystem
{
    /// <summary>
    /// The base class for each of your events. You can narrow which event
    /// goes with which dispatcher by creating a set of base classes inheriting from this base.
    /// </summary>
    public abstract class EventBase
    {
        public string Origin { get; protected set; } = string.Empty;

        public abstract void Copy(EventBase eventObject);
		
        public abstract string GetDebugData();

        /// <summary>
        /// Instant events aren't propagated to newly registered controllers, effectively making them "fire and forget".
        /// </summary>
        public virtual bool GetIsInstant()
        {
            return false;
        }

        public EventBase(string origin)
        {
            Origin = origin;
        }
    }

    /*
    How you assign your data to your events is up to you,
    but for an example, please see the dummy event found below. 

    public class DummyEvent : EventBase
    {
        public string Value1 { get; private set; } = string.Empty;
        public int Value2 { get; private set; } = 0;

        public void Reset(string value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public override void Copy(EventBase eventObject)
        {
            if (eventObject is DummyEvent dummyEvent)
            {
                Value1 = dummyEvent.Value1;
                Value2 = dummyEvent.Value2;
            }
        }

        public override string GetDebugData()
        {
            return $"[DummyEvent] sent by [{Origin}] : Data are [{Value1}] and [{Value2}].";
        }

        public DummyEvent(string origin)
            : base(origin) { }
    }
    */
}