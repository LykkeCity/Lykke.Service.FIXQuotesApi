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

        public override async Task<IReadOnlyCollection<FixQuoteModel>> GetAll()
        {
            var now = TimeService.UtcNow;
            var todaysQuotes = await GetAllOnT();
            if (todaysQuotes.Any(q => q.TradeTime > now))
            {
                return await GetAllOnTMinusOne();
            }
            return todaysQuotes;
        }

        public override async Task<FixQuoteModel> GetById(string id)
        {
            var now = TimeService.UtcNow;
            var todayQuote = await GetByIdOnT(id);
            if (todayQuote?.TradeTime > now)
            {
                return await GetByIdOnTMinusOne(id);
            }
            return todayQuote;
        }
    }
}