// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.ExtensionMethods;
using Softeq.Serilog.Extension;
using SerilogConfig = Serilog.LoggerConfiguration;

namespace Softeq.NetKit.Auth.Common.Logger
{
    public static class LoggerConfigurationExtensions
    {
        public static IWebHostBuilder UseLogger(this IWebHostBuilder hostBuilder,
            Func<WebHostBuilderContext, LoggerConfiguration> getLoggerConfiguration)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder =>
                SetupLogger(getLoggerConfiguration(context), context.Configuration, context.HostingEnvironment.EnvironmentName,
                    CheckHostingEnvironmentIsProductionOrStagingOrDevelopment(context.HostingEnvironment), collection)));
        }

        public static IHostBuilder UseLogger(this IHostBuilder hostBuilder, Func<HostBuilderContext, LoggerConfiguration> getLoggerConfiguration)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder =>
                SetupLogger(getLoggerConfiguration(context), context.Configuration, context.HostingEnvironment.EnvironmentName,
                    CheckHostingEnvironmentIsProductionOrStagingOrDevelopment(context.HostingEnvironment), collection)));
        }

        private static bool CheckHostingEnvironmentIsProductionOrStagingOrDevelopment(
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsProduction()
                   || hostingEnvironment.IsStaging()
                   || hostingEnvironment.IsDevelopment();
        }

        private static bool CheckHostingEnvironmentIsProductionOrStagingOrDevelopment(
            Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsProduction()
                   || hostingEnvironment.IsStaging()
                   || hostingEnvironment.IsDevelopment();
        }

        private static void SetupLogger(LoggerConfiguration loggerConfiguration, IConfiguration hostingContextConfiguration, string environmentName,
            bool isHostingEnvironmentProductionOrStagingOrDevelopment, IServiceCollection serviceCollection)
        {
            var applicationName = loggerConfiguration.SerilogApplicationName;
            var applicationSlotName = $"{applicationName}:{environmentName}";

            var serilogConfiguration = new SerilogConfig().ReadFrom.Configuration(hostingContextConfiguration)
                .Enrich.WithProperty(PropertyNames.Application, applicationSlotName);

            var template = GetLogTemplate();

            if (isHostingEnvironmentProductionOrStagingOrDevelopment)
            {
                var instrumentationKey = loggerConfiguration.ApplicationInsightsInstrumentationKey;
                var telemetryClient = new TelemetryClient {InstrumentationKey = instrumentationKey};
                serilogConfiguration.WriteTo.ApplicationInsights(telemetryClient, LogEventsToTelemetryConverter);

                serviceCollection.AddSingleton(telemetryClient);
            }
            else
            {
                serilogConfiguration.WriteTo.Debug(outputTemplate: template);
            }

            if (loggerConfiguration.SerilogEnableLocalFileSink)
            {
                serilogConfiguration.WriteTo.RollingFile("logs/log-{Date}.txt",
                    outputTemplate: template,
                    fileSizeLimitBytes: loggerConfiguration.SerilogFileSizeLimitMBytes * 1024 * 1024);
            }

            var logger = serilogConfiguration.CreateLogger();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                logger.Event("UnhandledExceptionCaughtByAppDomainUnhandledExceptionHandler")
                    .With.Message("Exception object = '{@ExceptionObject}'; Is terminating = '{IsTerminating}'", args.ExceptionObject,
                        args.IsTerminating)
                    .AsFatal();
            };

            Log.Logger = logger;
        }

        private static ITelemetry LogEventsToTelemetryConverter(LogEvent serilogLogEvent, IFormatProvider formatProvider)
        {
            if (serilogLogEvent.Exception == null)
            {
                if (serilogLogEvent.Properties.ContainsKey(PropertyNames.EventId))
                {
                    var eventTelemetry = new EventTelemetry(serilogLogEvent.Properties[PropertyNames.EventId].ToString())
                    {
                        Timestamp = serilogLogEvent.Timestamp
                    };
                    serilogLogEvent.ForwardPropertiesToTelemetryProperties(eventTelemetry, formatProvider);
                    return eventTelemetry;
                }

                var exceptionTelemetry = new ExceptionTelemetry(new Exception($"Event does not contain '{PropertyNames.EventId}' property"))
                {
                    Timestamp = serilogLogEvent.Timestamp
                };
                serilogLogEvent.ForwardPropertiesToTelemetryProperties(exceptionTelemetry, formatProvider);
                return exceptionTelemetry;
            }

            return serilogLogEvent.ToDefaultExceptionTelemetry(formatProvider);
        }

        private static string GetLogTemplate()
        {
            return new SerilogTemplateBuilder().Timestamp()
                .Level()
                .CorrelationId()
                .EventId()
                .Message()
                .NewLine()
                .Exception()
                .Build();
        }
    }
}