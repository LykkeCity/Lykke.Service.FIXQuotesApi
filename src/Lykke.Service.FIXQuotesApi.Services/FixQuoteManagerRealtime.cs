using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Lykke.Service.FIXQuotesApi.Core.Services;

namespace Lykke.Service.FIXQuotesApi.Services
{
    public sealed class FixQuoteManagerRealtime : FixQuoteManager, IFixQuoteManagerRealtime
    {
        public FixQuoteManagerRealtime(IFixQuoteRepository repository, ILog log, ITimeService timeService) : base(repository, timeService, log)
        {
        }

        public override Task<IReadOnlyCollection<FixQuote>> GetAll(DateTime date)
        {
            return GetAllImpl(date);
        }

        public override Task<FixQuote> GetById(DateTime date, string assetPair)
        {
            return GetAllImpl(date, assetPair);
        }
    }
}
