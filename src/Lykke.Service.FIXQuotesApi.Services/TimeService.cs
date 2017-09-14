using System;
using Lykke.Service.FIXQuotesApi.Core.Services;

namespace Lykke.Service.FIXQuotesApi.Services
{
    public sealed class TimeService : ITimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}