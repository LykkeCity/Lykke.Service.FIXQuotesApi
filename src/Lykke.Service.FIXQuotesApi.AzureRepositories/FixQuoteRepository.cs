using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using MoreLinq;

namespace Lykke.Service.FIXQuotesApi.AzureRepositories
{
    public class FixQuoteRepository : IFixQuoteRepository
    {
        private readonly INoSQLTableStorage<FixQuoteEntity> _storage;


        public FixQuoteRepository(INoSQLTableStorage<FixQuoteEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyCollection<FixQuote>> GetAllAsync(DateTime date)
        {
            var enitites = await _storage.GetDataAsync(FixQuoteEntity.ToPartitionKey(date));
            return enitites.Select(e => new FixQuote
            {
                Ask = e.Ask,
                Bid = e.Bid,
                TradeTime = e.TradeTime,
                FixingTime = e.FixingTime,
                AssetPair = e.Id
            }).ToArray();
        }

        public async Task<FixQuote> GetById(DateTime date, string assetPair)
        {
            assetPair = assetPair.ToUpperInvariant();
            var entity =
                await _storage.GetDataAsync(FixQuoteEntity.ToPartitionKey(date), FixQuoteEntity.ToRowKey(assetPair));
            if (entity != null)
            {
                return new FixQuote
                {
                    Ask = entity.Ask,
                    Bid = entity.Bid,
                    TradeTime = entity.TradeTime,
                    FixingTime = entity.FixingTime,
                    AssetPair = entity.Id
                };
            }
            return null;
        }

        public async Task SaveAsync(IEnumerable<FixQuote> quotes)
        {
            const int batchSize = 100;
            var entities = quotes.Select(q => new FixQuoteEntity(q.AssetPair.ToUpperInvariant(), q.Ask, q.Bid, q.FixingTime, q.TradeTime));
            foreach (var batch in entities.Batch(batchSize))
            {
                await _storage.InsertOrMergeBatchAsync(batch);
            }
        }
    }
}
