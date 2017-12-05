using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Lykke.Service.FIXQuotesApi.Core.Services;

namespace Lykke.Service.FIXQuotesApi.Services
{
    public abstract class FixQuoteManager : IFixQuoteManager
    {
        private const int DepthOfSearch = 7;
        private readonly IFixQuoteRepository _repository;
        protected readonly ITimeService TimeService;
        private readonly ILog _log;

        protected FixQuoteManager(IFixQuoteRepository repository, ITimeService timeService, ILog log)
        {
            _repository = repository;
            TimeService = timeService;
            _log = log;
        }


        public abstract Task<IReadOnlyCollection<FixQuote>> GetAll(DateTime date);
        public abstract Task<FixQuote> GetById(DateTime date, string assetPair);

        protected async Task<IReadOnlyCollection<FixQuote>> GetAllImpl(DateTime time)
        {
            for (var i = 0; i > -DepthOfSearch; i--)
            {
                var quotes = await _repository.GetAllAsync(time.AddDays(i));
                if (quotes.Count > 0)
                {
                    return quotes;
                }
            }
            await _log.WriteWarningAsync(nameof(FixQuoteManager), nameof(GetAllImpl), "Selecting fix quotes",
                $"Unable to find any quotes for last {DepthOfSearch} days");

            return new FixQuote[0];
        }

        protected async Task<FixQuote> GetAllImpl(DateTime time, string id)
        {
            for (var i = 0; i > -DepthOfSearch; i--)
            {
                var quote = await _repository.GetById(time.AddDays(i), id);
                if (quote != null)
                {
                    return quote;
                }
            }

            await _log.WriteWarningAsync(nameof(FixQuoteManager), nameof(GetAllImpl), "Selecting fix quote by Id",
                $"Unable to find a quote with Id {id} for last {DepthOfSearch} days");
            return null;
        }

    }
}
