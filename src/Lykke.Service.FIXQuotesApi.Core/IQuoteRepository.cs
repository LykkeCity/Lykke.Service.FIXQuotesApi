using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;

namespace Lykke.Service.FIXQuotesApi.Core
{
    public interface IQuoteRepository
    {
        Task<IReadOnlyCollection<FixQuoteModel>> GetAllAsync(DateTime date);
        Task<FixQuoteModel> GetById(DateTime date, string id);
        Task SaveAsync(IEnumerable<FixQuoteModel> quotes);
    }
}