// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;

namespace Softeq.NetKit.Auth.Jobs.Edc.Configurations
{
    public class HandleNotCompletedEdcEventsJobConfiguration
    {
        public HandleNotCompletedEdcEventsJobConfiguration(int maxAttemptsCountBeforeEmailSending, string serviceBusErrorEmailTemplateName,
            string serviceBusErrorEmailRecipients)
        {
            Ensure.That(maxAttemptsCountBeforeEmailSending).IsGt(0);
            Ensure.That(serviceBusErrorEmailTemplateName).IsNotNullOrWhiteSpace();
            Ensure.That(serviceBusErrorEmailRecipients).IsNotNullOrWhiteSpace();

            MaxAttemptsCountBeforeEmailSending = maxAttemptsCountBeforeEmailSending;
            ServiceBusErrorEmailTemplateName = serviceBusErrorEmailTemplateName;
            ServiceBusErrorEmailRecipients = serviceBusErrorEmailRecipients;
        }

        public int MaxAttemptsCountBeforeEmailSending { get; }

        public string ServiceBusErrorEmailTemplateName { get; }

        public string ServiceBusErrorEmailRecipients { get; }
    }
}