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
        Task<IReadOnlyCollection<FixQuoteModel>> GetAll();

        Task<FixQuoteModel> GetById(string id);
    }
}