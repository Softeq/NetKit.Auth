// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnsureThat;
using Softeq.NetKit.Auth.Integration.Email.Dto;
using Softeq.NetKit.Auth.Integration.Email.Exceptions;
using Softeq.NetKit.Auth.Integration.Email.Notifications;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Softeq.NetKit.Auth.Integration.Email
{
    internal class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            Ensure.That(emailConfiguration).IsNotNull();

            _emailConfiguration = emailConfiguration;
        }

        public async Task SendEmailAsync(SendEmailDto email)
        {
            try
            {
                var client = new SendGridClient(_emailConfiguration.SendGridApiKey);
                var message = new SendGridMessage()
                {
                    From = new EmailAddress(
                        string.IsNullOrWhiteSpace(email.FromEmail) ? _emailConfiguration.SenderEmail : email.FromEmail,
                        string.IsNullOrWhiteSpace(email.FromName) ? _emailConfiguration.SenderName : email.FromName),
                    Subject = email.Subject,
                    PlainTextContent = email.Text,
                    HtmlContent = email.HtmlText,
                };

                foreach (var recipient in email.Recipients)
                {
                    switch (recipient.DeliveryType)
                    {
                        case EmailDeliveryType.Regular:
                            message.AddTo(new EmailAddress(recipient.Email, recipient.Name));
                            break;
                        case EmailDeliveryType.Personal:
                            message.Personalizations = message.Personalizations ?? new List<Personalization>();
                            message.Personalizations.Add(new Personalization
                                {Tos = new List<EmailAddress> {new EmailAddress(recipient.Email, recipient.Name)}});
                            break;
                    }
                }

                await client.SendEmailAsync(message);
            }
            catch (Exception exception)
            {
                throw new EmailException("Error while sending email", exception);
            }
        }

        public async Task SendNotificationAsync(IEmailNotification emailNotification)
        {
            await SendEmailAsync(new SendEmailDto
            {
                FromName = _emailConfiguration.SenderName,
                FromEmail = _emailConfiguration.SenderEmail,
                Subject = emailNotification.Subject,
                Recipients = emailNotification.Recipients,
                Text = emailNotification.Text,
                HtmlText = emailNotification.GetHtml()
            });
        }
    }
}
