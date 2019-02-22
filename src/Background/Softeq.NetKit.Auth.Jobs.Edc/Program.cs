// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EnsureThat;
using Softeq.NetKit.Auth.Common.EmailTemplates;
using Softeq.NetKit.Auth.Common.Logger;
using Softeq.NetKit.Auth.Integration.Edc;
using Softeq.NetKit.Auth.Integration.Email;
using Softeq.NetKit.Auth.Jobs.Edc.Configurations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Softeq.NetKit.Integrations.EventLog;
using IntegrationEdcModule = Softeq.NetKit.Auth.Integration.Edc.DIModule;
using EmailEdcModule = Softeq.NetKit.Auth.Integration.Email.DIModule;
using LoggerConfiguration = Softeq.NetKit.Auth.Common.Logger.LoggerConfiguration;
using LoggerModule = Softeq.NetKit.Auth.Common.Logger.DIModule;
using CommonEmailTemplatesModule = Softeq.NetKit.Auth.Common.EmailTemplates.DIModule;

namespace Softeq.NetKit.Auth.Jobs.Edc
{
    public class Program
    {
        private const string GlobalEnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";

        public static void Main()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebJobs(config =>
                {
                    config
                        .AddAzureStorageCoreServices()
                        .AddTimers();
                })
                .ConfigureHostConfiguration(config => { config.AddEnvironmentVariables(); })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = Environment.GetEnvironmentVariable(GlobalEnvironmentVariableName);

                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();

                    config.AddConfiguration(configuration);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(context.Configuration);

                    services.AddSingleton<INameResolver, ConfigExpressionNameResolver>(_ => new ConfigExpressionNameResolver(context.Configuration));

                    services.AddDbContext<IntegrationEventLogContext>(options => options.UseSqlServer(context.Configuration["Database:ConnectionString"]));

                    var builder = new ContainerBuilder();
                    builder.Populate(services);

                    RegisterEdc(builder);
                    RegisterEmail(builder);
                    builder.RegisterModule<LoggerModule>();
                    builder.RegisterModule<CommonEmailTemplatesModule>();

                    builder
                        .Register(builderContext =>
                        {
                            var configuration = builderContext.Resolve<IConfiguration>();
                            var configurationSection = configuration.GetSection("Jobs:HandleNotCompletedEdcEvents");

                            return new HandleNotCompletedEdcEventsJobConfiguration(
                                int.Parse(configurationSection["MaxAttemptsCountBeforeEmailSending"]),
                                configurationSection["ServiceBusErrorEmailTemplateName"],
                                configurationSection["ServiceBusErrorEmailRecipients"]);
                        })
                        .AsSelf()
                        .SingleInstance();

                    builder
                        .Register(builderContext =>
                        {
                            var configuration = builderContext.Resolve<IConfiguration>();
                            var logEventId = configuration["Jobs:LogEventId"];

                            var handleNotCompletedEdcEventsJobConfiguration = builderContext.Resolve<HandleNotCompletedEdcEventsJobConfiguration>();

                            return new JobsConfiguration(logEventId, handleNotCompletedEdcEventsJobConfiguration);
                        })
                        .AsSelf()
                        .SingleInstance();

                    builder.RegisterType<Functions>();

                    var container = builder.Build();

                    services.AddSingleton<IJobActivator>(new AutofacJobActivator(container));

                    services.BuildServiceProvider();
                })
                .UseConsoleLifetime()
                .UseLogger(GetLoggerConfiguration);

            using (var host = hostBuilder.Build())
            {
                host.Run();
            }
        }

        private static void RegisterEmail(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();
                    return new EmailConfiguration(
                        configuration["SendGrid:APIKey"],
                        configuration["SendGrid:SenderEmail"],
                        configuration["SendGrid:SenderName"]);
                })
                .As<EmailConfiguration>()
                .SingleInstance();

            builder.Register(context =>
                {
                    var templatesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

                    return new EmailTemplateConfiguration(templatesFolderPath);
                })
                .As<EmailTemplateConfiguration>()
                .SingleInstance();

            builder.RegisterModule<EmailEdcModule>();
        }

        private static void RegisterEdc(ContainerBuilder builder)
        {
            builder
                .Register(componentContext =>
                {
                    var configuration = componentContext.Resolve<IConfiguration>();

                    var connectionString = configuration["AzureServiceBus:ConnectionString"];
                    Ensure.That(connectionString).IsNotNullOrWhiteSpace();

                    var topicClientName = configuration["AzureServiceBus:TopicName"];
                    Ensure.That(topicClientName).IsNotNullOrWhiteSpace();

                    var subscriptionClientName = configuration["AzureServiceBus:SubscriptionName"];
                    Ensure.That(subscriptionClientName).IsNotNullOrWhiteSpace();

                    var queueName = configuration["AzureServiceBus:QueueName"];
                    Ensure.That(queueName).IsNotNullOrWhiteSpace();

                    var messageTimeToLiveInHours = configuration["AzureServiceBus:MessageTimeToLiveInMinutes"];
                    Ensure.That(messageTimeToLiveInHours).IsNotNullOrWhiteSpace();

                    var eventPublisherId = configuration["AzureServiceBus:EventPublisherId"];
                    Ensure.That(eventPublisherId).IsNotNullOrWhiteSpace();

                    return new EdcConfiguration(connectionString, topicClientName, subscriptionClientName,
                        queueName, Convert.ToInt32(messageTimeToLiveInHours), eventPublisherId);
                })
                .As<EdcConfiguration>()
                .SingleInstance();

            builder.RegisterModule<IntegrationEdcModule>();
        }

        private static LoggerConfiguration GetLoggerConfiguration(HostBuilderContext context)
        {
            return new LoggerConfiguration(
                context.Configuration["ApplicationInsights:InstrumentationKey"],
                context.Configuration["Serilog:ApplicationName"],
                bool.Parse(context.Configuration["Serilog:EnableLocalFileSink"]),
                int.Parse(context.Configuration["Serilog:FileSizeLimitMBytes"]));
        }
    }
}