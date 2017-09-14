
using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.FIXQuotesApi.AzureRepositories;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using NUnit.Framework;
using System.Linq;

namespace Lykke.Service.FIXQuotesApi.Tests
{
    [TestFixture(Category = "Integration"), Explicit]
    public class FixQuoteRepositoryTest
    {
        private INoSQLTableStorage<FixQuoteEntity> _storage;
        private FixQuoteRepository _repository;

        private readonly FixQuoteModel _rur = new FixQuoteModel
        {
            Ask = 1,
            Bid = 2,
            AssetPair = "rur",
            TradeTime = DateTime.Now,
            FixingTime = DateTime.Now.AddDays(-1)
        };

        private readonly FixQuoteModel _usd = new FixQuoteModel
        {
            Ask = 3,
            Bid = 4,
            AssetPair = "usd",
            TradeTime = DateTime.Now,
            FixingTime = DateTime.Now.AddDays(-1)
        };

        [SetUp]
        public async Task SetUp()
        {
#pragma warning disable 618
            _storage = new AzureTableStorage<FixQuoteEntity>("UseDevelopmentStorage=true", "testTable", new LogToConsole());
#pragma warning restore 618
            _repository = new FixQuoteRepository(_storage);
            await Cleanup();
        }

        [TearDown]
        public async Task TearDown()
        {

            await Cleanup();
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
                new FixQuoteEntity("rur",1,2,DateTime.Now, DateTime.Now.AddDays(1)),
                new FixQuoteEntity("usd",1,2,DateTime.Now, DateTime.Now.AddDays(1))
            };
            await _storage.InsertAsync(quotes);

            var resutl = await _repository.GetAllAsync(DateTime.Now);

            Assert.That(resutl.Count, Is.EqualTo(2));
            Assert.That(resutl.Count(s => s.AssetPair == _rur.AssetPair), Is.EqualTo(1));
            Assert.That(resutl.Count(s => s.AssetPair == _usd.AssetPair), Is.EqualTo(1));
        }

        [TestCase("usd", "rur")]
        [TestCase("rur", "usd")]
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

        private async Task Cleanup()
        {
            await _storage.DeleteIfExistAsync(FixQuoteEntity.ToPartitionKey(DateTime.Now), FixQuoteEntity.ToRowKey(_rur.AssetPair));
            await _storage.DeleteIfExistAsync(FixQuoteEntity.ToPartitionKey(DateTime.Now), FixQuoteEntity.ToRowKey(_usd.AssetPair));
        }
    }
}