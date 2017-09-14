using System;
using Autofac;
using AzureStorage;
using AzureStorage.Tables;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.FIXQuotesApi.AzureRepositories;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Lykke.Service.FIXQuotesApi.Core.Services;
using Lykke.Service.FIXQuotesApi.Services;

namespace Lykke.Service.FIXQuotesApi.Modules
{
    internal class ServiceModule : Module
    {
        private readonly FIXQuotesApiSettings _settings;
        private readonly ILog _log;

        public ServiceModule(FIXQuotesApiSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            RegisterRabbitSubscriber(builder);

            RegisterAzureRepositories(builder);

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<FixQuoteManagerRealtime>()
                .As<IFixQuoteManagerRealtime>();

            builder.RegisterType<FixQuoteManagerDelayed>()
                .As<IFixQuoteManagerDelayed>();

            builder.RegisterType<QuoteUpdater>()
                .SingleInstance()
                .AsSelf();

            builder.RegisterType<TimeService>()
                .As<ITimeService>()
                .SingleInstance();
        }

        private void RegisterRabbitSubscriber(ContainerBuilder builder)
        {
            var reciverRabbitMqSettings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _settings.FixQuoteFeedRabbit.ConnectionString,
                QueueName = _settings.FixQuoteFeedRabbit.QueueName,
                ExchangeName = _settings.FixQuoteFeedRabbit.ExchangeName,
                IsDurable = false
            };

            var subscriber = new RabbitMqSubscriber<FixQuotePack>(reciverRabbitMqSettings,
                    new ResilientErrorHandlingStrategy(_log, reciverRabbitMqSettings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<FixQuotePack>())
                .SetMessageReadStrategy(new MessageReadWithTemporaryQueueStrategy())
                .SetLogger(_log);

            builder.Register(c => subscriber)
                .As<IMessageConsumer<FixQuotePack>>()
                .AsSelf()
                .SingleInstance();
        }


        private void RegisterAzureRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<FixQuoteRepository>()
                .As<IQuoteRepository>();

            builder.Register(c => new AzureTableStorage<FixQuoteEntity>(_settings.Db.BackupConnString, "fixQuotesBackup", _log))
                .As<INoSQLTableStorage<FixQuoteEntity>>();
        }
    }
}

