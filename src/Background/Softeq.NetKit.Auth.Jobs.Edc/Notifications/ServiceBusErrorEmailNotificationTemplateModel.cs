// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.Jobs.Edc.Notifications
{
	public class ServiceBusErrorEmailNotificationTemplateModel : IEmailNotificationTemplateModel
	{
		public ServiceBusErrorEmailNotificationTemplateModel(Guid eventId)
		{
			EventId = eventId;
		}

		public Guid EventId { get; set; }
	}
}