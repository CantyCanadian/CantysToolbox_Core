﻿namespace Canty.EventSystem
{
    /// <summary>
    /// The base class for each of your events. You can narrow which event
    /// goes with which dispatcher by creating a set of base classes inheriting from this base.
    /// </summary>
    public abstract class EventBase
    {
        public string Origin { get; private set; } = string.Empty;

        public abstract void Copy(EventBase other);
        public abstract string GetDebugData();

        public EventBase(string origin)
        {
            Origin = origin;
        }

        public EventBase() { }
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

        public override void Copy(EventBase other)
        {
            if (other is DummyEvent)
            {
                DummyEvent dummy = other as DummyEvent;

                Value1 = dummy.Value1;
                Value2 = dummy.Value2;
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