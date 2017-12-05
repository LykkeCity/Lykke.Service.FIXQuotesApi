using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;

namespace Lykke.Service.FIXQuotesApi.Core.Services
{
    /// <summary>
    /// Returns quotes to publish to consumers.
    /// </summary>
    public interface IFixQuoteManager
    {
        Task<IReadOnlyCollection<FixQuote>> GetAll(DateTime date);

        Task<FixQuote> GetById(DateTime date, string assetPair);
    }
}
