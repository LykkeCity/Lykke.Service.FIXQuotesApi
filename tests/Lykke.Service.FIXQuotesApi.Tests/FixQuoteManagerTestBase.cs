using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FIXQuotesApi.Core;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Lykke.Service.FIXQuotesApi.Core.Services;
using NSubstitute;

namespace Lykke.Service.FIXQuotesApi.Tests
{
    internal abstract  class FixQuoteManagerTestBase
    {
        protected IFixQuoteManager Manager;
        protected IFixQuoteRepository RepositoryMock;
        protected ITimeService TimeServiceMock;
        protected LogToConsole LogToConsole;

        public virtual void Setup()
        {
            RepositoryMock = Substitute.For<IFixQuoteRepository>();
            TimeServiceMock = Substitute.For<ITimeService>();
            LogToConsole = new LogToConsole();
        }

        protected void SetupQuote(DateTime fixingTime, DateTime tradeTime)
        {
            var q = new FixQuoteModel
            {
                FixingTime = fixingTime,
                TradeTime = tradeTime
            };
            RepositoryMock.GetAllAsync(fixingTime.Date).Returns(Task.FromResult<IReadOnlyCollection<FixQuoteModel>>(new[] { q }));
        }
    }
}