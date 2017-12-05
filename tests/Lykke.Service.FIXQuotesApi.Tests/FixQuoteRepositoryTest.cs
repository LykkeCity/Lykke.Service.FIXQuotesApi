
using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Lykke.Service.FIXQuotesApi.AzureRepositories;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using NUnit.Framework;
using System.Linq;
using Common.Log;
using Lykke.SettingsReader;
using NSubstitute;

namespace Lykke.Service.FIXQuotesApi.Tests
{
    [TestFixture]
    public class FixQuoteRepositoryTest
    {
        private INoSQLTableStorage<FixQuoteEntity> _storage;
        private FixQuoteRepository _repository;

        private readonly FixQuote _rur = new FixQuote
        {
            Ask = 1,
            Bid = 2,
            AssetPair = "RUR",
            TradeTime = DateTime.Now,
            FixingTime = DateTime.Now.AddDays(-1)
        };

        private readonly FixQuote _usd = new FixQuote
        {
            Ask = 3,
            Bid = 4,
            AssetPair = "USD",
            TradeTime = DateTime.Now,
            FixingTime = DateTime.Now.AddDays(-1)
        };

        [SetUp]
        public async Task SetUp()
        {
            _storage = new NoSqlTableInMemory<FixQuoteEntity>();
            _repository = new FixQuoteRepository(_storage);
        }

        [Test]
        public async Task ShouldSaveQuotes()
        {
            var quotes = new[]
            {
                _rur, _usd
            };

            await _repository.SaveAsync(quotes);

            var stored = await _storage.GetDataAsync();

            Assert.That(stored.Count, Is.EqualTo(2));
            Assert.That(stored.Count(s => s.Id == _rur.AssetPair), Is.EqualTo(1));
            Assert.That(stored.Count(s => s.Id == _usd.AssetPair), Is.EqualTo(1));

        }

        [Test]
        public async Task ShouldGetAll()
        {
            var quotes = new[]
            {
                new FixQuoteEntity("RUR",1,2,DateTime.Now, DateTime.Now.AddDays(1)),
                new FixQuoteEntity("USD",1,2,DateTime.Now, DateTime.Now.AddDays(1))
            };
            await _storage.InsertAsync(quotes);

            var resutl = await _repository.GetAllAsync(DateTime.Now);

            Assert.That(resutl.Count, Is.EqualTo(2));
            Assert.That(resutl.Count(s => s.AssetPair == _rur.AssetPair), Is.EqualTo(1));
            Assert.That(resutl.Count(s => s.AssetPair == _usd.AssetPair), Is.EqualTo(1));
        }

        [TestCase("USD", "RUR")]
        [TestCase("RUR", "USD")]
        public async Task ShouldGetById(string id1, string id2)
        {
            var quotes = new[]
            {
                new FixQuoteEntity(id1,1,2,DateTime.Now, DateTime.Now.AddDays(1)),
                new FixQuoteEntity(id2,1,2,DateTime.Now, DateTime.Now.AddDays(1))
            };
            await _storage.InsertAsync(quotes);

            var resutl = await _repository.GetById(DateTime.Now, id1);

            Assert.That(resutl, Is.Not.Null);
            Assert.That(resutl.AssetPair == id1, Is.True);
        }

        [Test]
        public async Task Convert()
        {
            var rm = Substitute.For<IReloadingManager<string>>();
            rm.Reload().Returns(Task.FromResult("DefaultEndpointsProtocol=https;AccountName=lkedevshared;AccountKey=deMBXlOj0telRE7poKY+2P9MwPcSsOJ+1IUqG6xoQ3kdDAMhxzvh22zKAXnlaKyPeF0gpbzgK4k7M9ZLdlTx+w=="));
            var storage = AzureTableStorage<FixQuoteEntity>.Create(rm, "fixQuotesBackup", new LogToConsole());
            var from = new DateTime(2017, 09, 01);
            for (; from < DateTime.Today.AddDays(1); from = from.AddDays(1))
            {
                var part = FixQuoteEntity.ToPartitionKey(from);

                var quotes = storage[part];
                foreach (var quote in quotes)
                {
                    var oldKey = quote.RowKey;
                    quote.RowKey = oldKey.ToUpperInvariant();
                    quote.Id = quote.RowKey;
                    await storage.InsertAsync(quote);
                    var deleted = await storage.DeleteAsync(part, oldKey);
                }
            }
        }
    }
}
