// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;
using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.Jobs.Edc.Notifications
{
	public class ServiceBusErrorEmailNotification: BaseEmailNotification<ServiceBusErrorEmailNotificationTemplateModel>
	{
	    public ServiceBusErrorEmailNotification(
	        ServiceBusErrorEmailNotificationTemplateModel templateModel,
	        string baseHtmlTemplate,
	        string htmlTemplate,
	        params RecipientDto[] recipients) : base(recipients)
	    {
	        Ensure.That(templateModel).IsNotNull();
	        Ensure.That(baseHtmlTemplate).IsNotNullOrWhiteSpace();
	        Ensure.That(htmlTemplate).IsNotNullOrWhiteSpace();
	        Ensure.That(recipients).IsNotNull();

	        TemplateModel = templateModel;
	        Subject = "Service Bus message handle failed";
	        BaseHtmlTemplate = baseHtmlTemplate;
	        HtmlTemplate = htmlTemplate;
	    }
	}
}