using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Lykke.Service.FIXQuotesApi.Core.Services;

namespace Lykke.Service.FIXQuotesApi.Services
{
    public sealed class FixQuoteManagerDelayed : FixQuoteManager, IFixQuoteManagerDelayed
    {
        public FixQuoteManagerDelayed(IFixQuoteRepository repository, ILog log, ITimeService timeService) : base(repository, timeService, log)
        {
        }

        public override async Task<IReadOnlyCollection<FixQuote>> GetAll(DateTime date)
        {
            var now = TimeService.UtcNow;
            var todaysQuotes = await GetAllImpl(date);
            if (todaysQuotes.Any(q => q.TradeTime > now))
            {
                return await GetAllImpl(now.Date.AddDays(-1));
            }
            return todaysQuotes;
        }

        public override async Task<FixQuote> GetById(DateTime date, string assetPair)
        {
            var now = TimeService.UtcNow;
            var todayQuote = await GetAllImpl(date, assetPair);
            if (todayQuote?.TradeTime > now)
            {
                return await GetAllImpl(now.Date.AddDays(-1), assetPair);
            }
            return todayQuote;
        }
    }
}
