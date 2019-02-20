// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Notifications;

namespace Softeq.NetKit.Auth.AppServices.EmailNotifications
{
    public class ResetPasswordEmailNotification : BaseEmailNotification<ResetPasswordEmailModel>
    {
        public ResetPasswordEmailNotification(
                        ResetPasswordEmailModel model,
                        RecipientDto recipients,
						string baseHtmlTemplate,
						string htmlTemplate) : base(recipients)
        {
            TemplateModel = model;
            Subject = "Password Reset Request";
	        BaseHtmlTemplate = baseHtmlTemplate;
	        HtmlTemplate = htmlTemplate;
        }
    }
}
