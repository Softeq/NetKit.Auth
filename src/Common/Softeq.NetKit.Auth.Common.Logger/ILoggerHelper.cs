// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Common.Logger
{
    public interface ILoggerHelper
    {
        void ResetCorrelationContextAndCorrelationId(string correlationId);
    }
}