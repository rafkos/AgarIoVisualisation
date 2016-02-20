using System;

namespace Assets.Scripts.Services
{
    public class SubscriptionToken
    {
        internal SubscriptionToken(Type eventType, WeakActionReference actionReference)
        {
            EventType = eventType;
            ActionReference = actionReference;
        }

        internal Type EventType { get; private set; }

        internal WeakActionReference ActionReference { get; private set; }
    }
}