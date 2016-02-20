using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Services
{
    internal class EventSubscribers
    {
        private readonly IList<EventTypeWithActions> _eventTypesWithActions;

        public EventSubscribers()
        {
            _eventTypesWithActions = new List<EventTypeWithActions>();
        }

        public SubscriptionToken Add<TEventType>(Action<TEventType> action) where TEventType : IEvent
        {
            var eventType = typeof (TEventType);
            WeakActionReference actionReference;

            lock (_eventTypesWithActions)
            {
                if (_eventTypesWithActions.All(i => i.EventType != eventType))
                {
                    _eventTypesWithActions.Add(new EventTypeWithActions(eventType));
                }
                var actionTarget = action.Target;
                var actionMethod = action.Method;

                actionReference = new WeakActionReference(actionTarget, actionMethod);

                _eventTypesWithActions.Single(i => i.EventType == eventType).Actions.Add(actionReference);
            }

            return new SubscriptionToken(eventType, actionReference);
        }

        public void Remove(SubscriptionToken subscriptionToken)
        {
            lock (_eventTypesWithActions)
            {
                var eventWithActions = _eventTypesWithActions.FirstOrDefault(i => i.EventType == subscriptionToken.EventType);

                if (eventWithActions == null)
                {
                    return;
                }

                eventWithActions.Actions.Remove(subscriptionToken.ActionReference);

                if (eventWithActions.Actions.Count == 0)
                {
                    _eventTypesWithActions.Remove(eventWithActions);
                }
            }
        }

        public void PublishEvent(IEvent publishEvent)
        {
            IList<WeakActionReference> actionsToInvoke;
            lock (_eventTypesWithActions)
            {
                var eventTypeWithActions = _eventTypesWithActions.FirstOrDefault(i => i.EventType == publishEvent.GetType());
                actionsToInvoke = eventTypeWithActions != null ? eventTypeWithActions.Actions.ToList() : null;
            }

            if (actionsToInvoke == null)
            {
                return;
            }

            foreach (var action in actionsToInvoke)
            {
                action.Invoke(publishEvent);
            }

            var deadActions = actionsToInvoke.Where(a => !a.IsAlive);

            lock (_eventTypesWithActions)
            {
                var eventTypeWithActions = _eventTypesWithActions.FirstOrDefault(i => i.EventType == publishEvent.GetType());

                if (eventTypeWithActions == null)
                {
                    return;
                }

                foreach (var deadAction in deadActions)
                {
                    eventTypeWithActions.Actions.Remove(deadAction);
                }

                if (eventTypeWithActions.Actions.Count == 0)
                {
                    _eventTypesWithActions.Remove(eventTypeWithActions);
                }
            }
        }
    }
}