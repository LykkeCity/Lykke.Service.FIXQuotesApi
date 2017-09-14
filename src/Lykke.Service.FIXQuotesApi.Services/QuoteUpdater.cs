using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;

namespace Lykke.Service.FIXQuotesApi.Services
{
    /// <summary>
    /// Listens updates with new quotes from the queue and save them to the Db
    /// </summary>
    public sealed class QuoteUpdater
    {
        private readonly IQuoteRepository _repository;
        private readonly ILog _log;

        public QuoteUpdater(IMessageConsumer<FixQuotePack> messageConsumer, IQuoteRepository repository, ILog log)
        {
            messageConsumer.Subscribe(QuoteReceivedCallback);
            _repository = repository;
            _log = log;
        }

        private async Task QuoteReceivedCallback(FixQuotePack quotePack)
        {
            if (quotePack.Quotes == null || quotePack.Quotes.Length == 0)
            {
                await _log.WriteWarningAsync(nameof(QuoteUpdater), nameof(QuoteReceivedCallback),
                    "Received a quote pack", "The quote pack contains no quotes");
            }
            await _repository.SaveAsync(quotePack.Quotes);
        }
    }
}