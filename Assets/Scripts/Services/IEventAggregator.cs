using System;

namespace Assets.Scripts.Services
{
    public interface IEventAggregator
    {
        SubscriptionToken Subscribe<TEventType>(Action<TEventType> action) where TEventType : IEvent;

        void Unsubscribe<TEventType>(SubscriptionToken subscriptionToken) where TEventType : IEvent;

        void Publish<TEventType>(TEventType publishEvent) where TEventType : IEvent;
    }
}