// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using CorrelationId;
using EnsureThat;
using Serilog;

namespace Softeq.NetKit.Auth.Common.Logger
{
    public class DIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CorrelationContextAccessor>()
                .As<ICorrelationContextAccessor>()
                .SingleInstance();

            builder.RegisterType<CorrelationContextFactory>()
                .As<ICorrelationContextFactory>()
                .InstancePerDependency();

            builder.RegisterType<LoggerHelper>()
                .As<ILoggerHelper>()
                .InstancePerDependency();

            builder.Register(context =>
                {
                    var correlationContextAccessor = context.Resolve<ICorrelationContextAccessor>();
                    Ensure.That(correlationContextAccessor).IsNotNull();

                    return Log.Logger.ForContext(new CorrelationIdEnricher(correlationContextAccessor));
                })
                .As<ILogger>()
                .InstancePerLifetimeScope();
        }
    }
}