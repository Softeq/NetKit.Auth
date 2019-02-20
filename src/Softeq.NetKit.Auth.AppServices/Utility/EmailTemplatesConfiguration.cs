// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;

namespace Softeq.NetKit.Auth.AppServices.Utility
{
    public class EmailTemplatesConfiguration
    {
        public EmailTemplatesConfiguration(string emailConfirmationEmailTemplateName, string resetPasswordEmailTemplateName,
            string changePasswordEmailTemplateName, string passwordHasExpiredEmailTemplateName)
        {
            Ensure.That(emailConfirmationEmailTemplateName).IsNotNullOrWhiteSpace();
            Ensure.That(resetPasswordEmailTemplateName).IsNotNullOrWhiteSpace();
            Ensure.That(changePasswordEmailTemplateName).IsNotNullOrWhiteSpace();
            Ensure.That(passwordHasExpiredEmailTemplateName).IsNotNullOrWhiteSpace();

            EmailConfirmationEmailTemplateName = emailConfirmationEmailTemplateName;
            ResetPasswordEmailTemplateName = resetPasswordEmailTemplateName;
            ChangePasswordEmailTemplateName = changePasswordEmailTemplateName;
            PasswordHasExpiredEmailTemplateName = passwordHasExpiredEmailTemplateName;
        }

        public string EmailConfirmationEmailTemplateName { get; }

        public string ResetPasswordEmailTemplateName { get; }

        public string ChangePasswordEmailTemplateName { get; }

        public string PasswordHasExpiredEmailTemplateName { get; }
    }
}