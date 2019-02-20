// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.AppServices.EmailNotifications
{
	public class ChangePasswordEmailNotification : BaseEmailNotification<ChangePasswordEmailModel>
	{
		public ChangePasswordEmailNotification(
			ChangePasswordEmailModel model,
			RecipientDto recipients,
			string baseHtmlTemplate,
			string htmlTemplate) : base(recipients)
		{
			TemplateModel = model;
			Subject = "Change Password";
			HtmlTemplate = htmlTemplate;
			BaseHtmlTemplate = baseHtmlTemplate;
		}
	}
}