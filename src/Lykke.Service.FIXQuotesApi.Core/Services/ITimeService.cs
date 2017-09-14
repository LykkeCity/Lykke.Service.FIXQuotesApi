using System;

namespace Lykke.Service.FIXQuotesApi.Core.Services
{
    public interface ITimeService
    {
        DateTime UtcNow { get; }
    }
}