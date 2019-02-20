// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.AppServices.EmailNotifications
{
    public class UserConfirmationEmailNotification : BaseEmailNotification<UserConfirmationEmailModel>
    {
		public UserConfirmationEmailNotification(
		                UserConfirmationEmailModel model,
		                string htmlTemplate,
		                string baseHtmlTemplate,
						params RecipientDto[] recipients) : base(recipients)
        {
		    TemplateModel = model;
		    Subject = "Email confirmation";
	        BaseHtmlTemplate = baseHtmlTemplate;
	        HtmlTemplate = htmlTemplate;
		}
	}
}
