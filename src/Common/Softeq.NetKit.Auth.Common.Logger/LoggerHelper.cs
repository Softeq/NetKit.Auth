// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CorrelationId;
using EnsureThat;

namespace Softeq.NetKit.Auth.Common.Logger
{
    internal class LoggerHelper : ILoggerHelper
    {
        private readonly ICorrelationContextFactory _correlationContextFactory;

        public LoggerHelper(ICorrelationContextFactory correlationContextFactory)
        {
            Ensure.That(correlationContextFactory).IsNotNull();
            
            _correlationContextFactory = correlationContextFactory;
        }

        public void ResetCorrelationContextAndCorrelationId(string correlationId)
        {
            // TODO: Find better way to set correlation id to the logger
            _correlationContextFactory.Create(Guid.NewGuid().ToString(), "CorrelationId");
        }
    }
}