// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.NetKit.Components.EventBus.Events;

namespace Softeq.NetKit.Auth.Integration.Edc.Services
{
    public interface IEdcPublishService
    {
	    Task PublishAsync(IntegrationEvent evt);
        Task RepublishAsync(Guid eventId, int? delayInSeconds = null);
    }
}
