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
        private readonly IQuoteRepository _repository;
        protected readonly ITimeService TimeService;
        private readonly ILog _log;

        protected FixQuoteManager(IQuoteRepository repository,ITimeService timeService, ILog log)
        {
            _repository = repository;
            TimeService = timeService;
            _log = log;
        }

        public abstract Task<IReadOnlyCollection<FixQuoteModel>> GetAll();
        public abstract Task<FixQuoteModel> GetById(string id);


        protected async Task<IReadOnlyCollection<FixQuoteModel>> GetAllOnT()
        {
            return await GetAll(0);
        }

        protected async Task<FixQuoteModel> GetByIdOnT(string id)
        {
            return await GetById(0, id);
        }

        protected async Task<FixQuoteModel> GetByIdOnTMinusOne(string id)
        {
            return await GetById(-1, id);
        }

        protected async Task<IReadOnlyCollection<FixQuoteModel>> GetAllOnTMinusOne()
        {
            return await GetAll(-1);
        }

        private async Task<IReadOnlyCollection<FixQuoteModel>> GetAll(int shift)
        {
            for (int i = 0; i < DepthOfSearch; i++)
            {
                var quotes = await _repository.GetAllAsync(TimeService.UtcNow.Date.AddDays(-i + shift));
                if (quotes.Count > 0)
                {
                    return quotes;
                }
            }
            await _log.WriteWarningAsync(nameof(FixQuoteManager), nameof(GetAll), "Selecting fix quotes",
                $"Unable to find any quotes for last {DepthOfSearch} days");

            return new FixQuoteModel[0];
        }

        private async Task<FixQuoteModel> GetById(int shift, string id)
        {
            for (int i = 0; i < DepthOfSearch; i++)
            {
                var quote = await _repository.GetById(TimeService.UtcNow.Date.AddDays(-i + shift), id);
                if (quote != null)
                {
                    return quote;
                }
            }

            await _log.WriteWarningAsync(nameof(FixQuoteManager), nameof(GetById), "Selecting fix quote by Id",
                $"Unable to find a quote with Id {id} for last {DepthOfSearch} days");
            return null;
        }
    }
}