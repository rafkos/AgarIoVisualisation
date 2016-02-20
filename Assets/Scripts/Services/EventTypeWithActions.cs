using System;
using System.Collections.Generic;

namespace Assets.Scripts.Services
{
    internal class EventTypeWithActions
    {
        public EventTypeWithActions(Type eventType)
        {
            ValidateEventType(eventType);
            EventType = eventType;
            Actions = new List<WeakActionReference>();
        }

        public Type EventType { get; private set; }

        public IList<WeakActionReference> Actions { get; private set; }

        private static void ValidateEventType(Type eventType)
        {
            if (!typeof (IEvent).IsAssignableFrom(eventType))
            {
                throw new EventTypeMismatchException();
            }
        }
    }
}