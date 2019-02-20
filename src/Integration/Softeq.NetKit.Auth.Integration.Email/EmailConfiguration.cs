// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;

namespace Softeq.NetKit.Auth.Integration.Email
{
    public class EmailConfiguration
    {
        public EmailConfiguration(string sendGridApiKey, string senderEmail, string senderName)
        {
            Ensure.That(sendGridApiKey).IsNotNullOrWhiteSpace();
            Ensure.That(senderEmail).IsNotNullOrWhiteSpace();

            SendGridApiKey = sendGridApiKey;
            SenderEmail = senderEmail;
            SenderName = senderName;
        }

        public string SendGridApiKey { get; }

        public string SenderEmail { get; }

        public string SenderName { get; }
    }
}