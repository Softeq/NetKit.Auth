// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using EnsureThat;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.Components.EventBus.Events;
using Softeq.NetKit.Integrations.EventLog;
using Softeq.NetKit.Integrations.EventLog.Abstract;

namespace Softeq.NetKit.Auth.Integration.Edc.Services
{
    internal class EdcPublishService : IEdcPublishService
    {
        private readonly IEventBusPublisher _eventBusPublisher;
        private readonly IIntegrationEventLogService _eventLogService;

        public EdcPublishService(
            IEventBusPublisher eventBusPublisher,
            IIntegrationEventLogService eventLogService)
        {
            Ensure.That(eventBusPublisher).IsNotNull();
            Ensure.That(eventLogService).IsNotNull();

            _eventBusPublisher = eventBusPublisher;
            _eventLogService = eventLogService;
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            await _eventLogService.SaveAsync(@event);
            await _eventBusPublisher.PublishToTopicAsync(@event);
            await _eventLogService.MarkAsPublishedAsync(@event);
        }

        public async Task RepublishAsync(Guid eventId, int? delayInSeconds = null)
        {
            var eventLog = await _eventLogService.GetAsync(eventId);

            if (eventLog.StateId != (int) EventStateEnum.Completed)
            {
                await _eventBusPublisher.PublishToTopicAsync(eventLog.Content, delayInSeconds);
                await _eventLogService.MarkAsPublishedAsync(eventLog.Content);
            }
        }
    }
}