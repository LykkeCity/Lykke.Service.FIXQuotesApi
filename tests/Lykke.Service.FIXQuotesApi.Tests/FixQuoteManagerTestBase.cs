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
        protected IFixQuoteManager _manager;
        protected IQuoteRepository _repositoryMock;
        protected ITimeService _timeServiceMock;
        protected LogToConsole _logToConsole;

        public virtual void Setup()
        {
            _repositoryMock = Substitute.For<IQuoteRepository>();
            _timeServiceMock = Substitute.For<ITimeService>();
            _logToConsole = new LogToConsole();
        }

        protected void SetupQuote(DateTime fixingTime, DateTime tradeTime)
        {
            var q = new FixQuoteModel
            {
                FixingTime = fixingTime,
                TradeTime = tradeTime
            };
            _repositoryMock.GetAllAsync(fixingTime.Date).Returns(Task.FromResult<IReadOnlyCollection<FixQuoteModel>>(new[] { q }));
        }
    }
}