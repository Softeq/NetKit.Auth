// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using EnsureThat;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.Components.EventBus.Events;
using Softeq.NetKit.Integrations.EventLog.Abstract;

namespace Softeq.NetKit.Auth.Integration.Edc.Handlers
{
    public class CompletedEventHandler : IEventHandler<CompletedEvent>
    {
	    private readonly IIntegrationEventLogService _integrationEventLogService;
        private readonly EdcConfiguration _edcConfiguration;

        public CompletedEventHandler(IIntegrationEventLogService integrationEventLogService,
            EdcConfiguration edcConfiguration)
	    {
            Ensure.That(integrationEventLogService).IsNotNull();
            Ensure.That(edcConfiguration).IsNotNull();

		    _integrationEventLogService = integrationEventLogService;
	        _edcConfiguration = edcConfiguration;
	    }

	    public async Task Handle(CompletedEvent @event)
	    {
	        if (@event.CompletedEventPublisherId == _edcConfiguration.EventPublisherId)
	        {
	            await _integrationEventLogService.CompleteAsync(@event.CompletedEventId);
	        }
	    }
	}
}
