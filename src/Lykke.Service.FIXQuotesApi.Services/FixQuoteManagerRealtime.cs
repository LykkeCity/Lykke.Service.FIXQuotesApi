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

        public override Task<IReadOnlyCollection<FixQuoteModel>> GetAll()
        {
            return GetAllOnT();
        }

        public override Task<FixQuoteModel> GetById(string id)
        {
            return GetByIdOnT(id);
        }
    }
}