// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using EnsureThat;
using Softeq.NetKit.Auth.Integration.Edc.Handlers;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.Components.EventBus.Events;

namespace Softeq.NetKit.Auth.Integration.Edc.Services
{
    internal class EdcSubscriptionService : IEdcSubscriptionService
    {
        private readonly IEventBusSubscriber _eventBusSubscriber;

        public EdcSubscriptionService(IEventBusSubscriber eventBusSubscriber)
        {
            Ensure.That(eventBusSubscriber).IsNotNull();

            _eventBusSubscriber = eventBusSubscriber;
        }

        public async Task SubscribeAsync()
        {
            await _eventBusSubscriber.RegisterSubscriptionListenerAsync();

            await _eventBusSubscriber.SubscribeAsync<CompletedEvent, CompletedEventHandler>();
        }
    }
}