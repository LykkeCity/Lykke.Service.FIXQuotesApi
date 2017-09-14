namespace Lykke.Service.FIXQuotesApi.Core
{
    public class AppSettings
    {
        public FIXQuotesApiSettings FIXQuotesApiService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }

    }

    public class FIXQuotesApiSettings
    {
        public DbSettings Db { get; set; }
        public string Secret { get; set; }
        public RabbitSettings FixQuoteFeedRabbit { get; set; }
    }

    public class DbSettings
    {
        public string LogsConnString { get; set; }
        public string BackupConnString { get; set; }
    }

    public class SlackNotificationsSettings
    {
        public AzureQueueSettings AzureQueue { get; set; }
    }

    public class AzureQueueSettings
    {
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }
    }

    public class RabbitSettings
    {
        public string ConnectionString { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
    }
}
