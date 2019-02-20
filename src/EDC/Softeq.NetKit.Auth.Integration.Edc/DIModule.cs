// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.NetKit.Auth.Integration.Edc.Handlers;
using Softeq.NetKit.Auth.Integration.Edc.Services;
using Softeq.NetKit.Components.EventBus;
using Softeq.NetKit.Components.EventBus.Abstract;
using Softeq.NetKit.Components.EventBus.Managers;
using Softeq.NetKit.Components.EventBus.Service;
using Softeq.NetKit.Components.EventBus.Service.Connection;
using Softeq.NetKit.Integrations.EventLog;
using Softeq.NetKit.Integrations.EventLog.Abstract;

namespace Softeq.NetKit.Auth.Integration.Edc
{
    public class DIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var edcConfiguration = context.Resolve<EdcConfiguration>();

                    return new ServiceBusPersisterConnectionConfiguration
                    {
                        ConnectionString = edcConfiguration.ConnectionString,
                        TopicConfiguration = new ServiceBusPersisterTopicConnectionConfiguration
                        {
                            TopicName = edcConfiguration.TopicName,
                            SubscriptionName = edcConfiguration.SubscriptionName,
                        },
                        QueueConfiguration = new ServiceBusPersisterQueueConnectionConfiguration
                        {
                            QueueName = edcConfiguration.QueueName
                        }
                    };
                })
                .As<ServiceBusPersisterConnectionConfiguration>()
                .SingleInstance();

            builder
                .RegisterType<ServiceBusPersisterConnection>()
                .As<IServiceBusPersisterConnection>()
                .SingleInstance();

            builder
                .RegisterType<EventBusSubscriptionsManager>()
                .As<IEventBusSubscriptionsManager>()
                .SingleInstance();

            builder
                .RegisterType<DeadLetterQueueMessagesManager>()
                .As<IDeadLetterQueueMessagesManager>()
                .SingleInstance();

            builder
                .Register(context =>
                {
                    var edcConfiguration = context.Resolve<EdcConfiguration>();

                    return new MessageQueueConfiguration
                    {
                        TimeToLiveInMinutes = edcConfiguration.MessageTimeToLiveInMinutes
                    };
                })
                .As<MessageQueueConfiguration>();

            builder
                .Register(context =>
                {
                    var edcConfiguration = context.Resolve<EdcConfiguration>();

                    return new EventPublishConfiguration(edcConfiguration.EventPublisherId);
                })
                .As<EventPublishConfiguration>();

            builder
                .RegisterType<EventBusService>()
                .As<IEventBusPublisher>()
                .As<IEventBusSubscriber>()
                .SingleInstance();

            builder
                .RegisterType<IntegrationEventLogService>()
                .As<IIntegrationEventLogService>();

            builder
                .RegisterType<EdcSubscriptionService>()
                .As<IEdcSubscriptionService>();

            builder
                .RegisterType<EdcPublishService>()
                .As<IEdcPublishService>();

            builder
                .RegisterType<CompletedEventHandler>()
                .AsSelf();
        }
    }
}