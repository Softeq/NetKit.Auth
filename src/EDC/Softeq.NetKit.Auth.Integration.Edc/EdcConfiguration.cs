// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.Auth.Integration.Edc
{
    public class EdcConfiguration
    {
        public EdcConfiguration(string connectionString, string topicName, string subscriptionName,
            string queueName, int messageTimeToLiveInMinutes, string eventPublisherId)
        {
            ConnectionString = connectionString;
            TopicName = topicName;
            SubscriptionName = subscriptionName;
            QueueName = queueName;
            MessageTimeToLiveInMinutes = messageTimeToLiveInMinutes;
            EventPublisherId = eventPublisherId;
        }

        public string ConnectionString { get; }

        public string TopicName { get; }

        public string SubscriptionName { get; }

        public string QueueName { get; }

        public int MessageTimeToLiveInMinutes { get; }

        public string EventPublisherId { get; }
    }
}