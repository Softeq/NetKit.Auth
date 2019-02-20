// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;

namespace Softeq.NetKit.Auth.Jobs.Edc.Configurations
{
    public class JobsConfiguration
    {
        public JobsConfiguration(string logEventId,
            HandleNotCompletedEdcEventsJobConfiguration handleNotCompletedEdcEventsJobConfiguration)
        {
            Ensure.That(logEventId).IsNotNullOrWhiteSpace();
            Ensure.That(handleNotCompletedEdcEventsJobConfiguration).IsNotNull();

            LogEventId = logEventId;
            HandleNotCompletedEdcEvents = handleNotCompletedEdcEventsJobConfiguration;
        }

        public string LogEventId { get; }

        public HandleNotCompletedEdcEventsJobConfiguration HandleNotCompletedEdcEvents { get; }
    }
}