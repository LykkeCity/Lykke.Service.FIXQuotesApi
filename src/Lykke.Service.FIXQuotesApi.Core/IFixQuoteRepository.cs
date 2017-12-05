using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;

namespace Lykke.Service.FIXQuotesApi.Core
{
    public interface IFixQuoteRepository
    {
        Task<IReadOnlyCollection<FixQuote>> GetAllAsync(DateTime date);
        Task<FixQuote> GetById(DateTime date, string assetPair);
        Task SaveAsync(IEnumerable<FixQuote> quotes);
    }
}