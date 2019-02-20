// Developed by Softeq Development Corporation
// http://www.softeq.com

using EnsureThat;

namespace Softeq.NetKit.Auth.Common.Logger
{
    public class LoggerConfiguration
    {
        public LoggerConfiguration(string applicationInsightsInstrumentationKey, string serilogApplicationName, bool serilogEnableLocalFileSink,
            int serilogFileSizeLimitMBytes)
        {
            Ensure.That(applicationInsightsInstrumentationKey).IsNotNullOrWhiteSpace();
            Ensure.That(serilogApplicationName).IsNotNullOrWhiteSpace();

            if (serilogEnableLocalFileSink)
            {
                Ensure.That(serilogFileSizeLimitMBytes).IsGte(0);
            }

            ApplicationInsightsInstrumentationKey = applicationInsightsInstrumentationKey;
            SerilogApplicationName = serilogApplicationName;
            SerilogEnableLocalFileSink = serilogEnableLocalFileSink;
            SerilogFileSizeLimitMBytes = serilogFileSizeLimitMBytes;
        }

        public string ApplicationInsightsInstrumentationKey { get; }

        public string SerilogApplicationName { get; }

        public bool SerilogEnableLocalFileSink { get; }

        public int SerilogFileSizeLimitMBytes { get; }
    }
}