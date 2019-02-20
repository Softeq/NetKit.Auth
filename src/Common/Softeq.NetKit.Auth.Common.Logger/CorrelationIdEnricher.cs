// Developed by Softeq Development Corporation
// http://www.softeq.com

using CorrelationId;
using EnsureThat;
using Serilog.Core;
using Serilog.Events;
using Softeq.Serilog.Extension;

namespace Softeq.NetKit.Auth.Common.Logger
{
    internal class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly ICorrelationContextAccessor _correlationIdAccessor;

        public CorrelationIdEnricher(ICorrelationContextAccessor correlationIdAccessor)
        {
            Ensure.That(correlationIdAccessor).IsNotNull();

            _correlationIdAccessor = correlationIdAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = _correlationIdAccessor.CorrelationContext?.CorrelationId;
            var correlationIdProperty = new LogEventProperty(PropertyNames.CorrelationId, new ScalarValue(correlationId ?? "unknown"));

            logEvent.AddPropertyIfAbsent(correlationIdProperty);
        }
    }
}