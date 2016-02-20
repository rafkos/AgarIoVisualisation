using System;

namespace Assets.Scripts.Services
{
    public class EventAggregator : IEventAggregator
    {
        private readonly EventSubscribers _eventSubscribers;

        public EventAggregator()
        {
            _eventSubscribers = new EventSubscribers();
        }

        public SubscriptionToken Subscribe<TEventType>(Action<TEventType> action) where TEventType : IEvent
        {
            return _eventSubscribers.Add(action);
        }

        public void Unsubscribe<TEventType>(SubscriptionToken subscriptionToken) where TEventType : IEvent
        {
            _eventSubscribers.Remove(subscriptionToken);
        }

        public void Publish<TEventType>(TEventType publishEvent) where TEventType : IEvent
        {
            _eventSubscribers.PublishEvent(publishEvent);
        }
    }
}