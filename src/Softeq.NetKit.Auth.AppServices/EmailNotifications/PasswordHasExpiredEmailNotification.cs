// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.AppServices.EmailNotifications
{
	public class PasswordHasExpiredEmailNotification : BaseEmailNotification<PasswordHasExpiredEmailModel>
	{
		public PasswordHasExpiredEmailNotification(
			PasswordHasExpiredEmailModel model,
			RecipientDto recipient,
			string baseHtmlTemplate,
			string htmlTemplate) : base(recipient)
		{
			TemplateModel = model;
			Subject = "Password Expired";
			HtmlTemplate = htmlTemplate;
			BaseHtmlTemplate = baseHtmlTemplate;

		}
	}
}