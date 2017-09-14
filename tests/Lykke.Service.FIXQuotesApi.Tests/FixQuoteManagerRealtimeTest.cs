using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.FIXQuotesApi.Services;
using NSubstitute;
using NUnit.Framework;

namespace Lykke.Service.FIXQuotesApi.Tests
{
    [TestFixture]
    internal class FixQuoteManagerRealtimeTest: FixQuoteManagerTestBase
    {

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _manager = new FixQuoteManagerRealtime(_repositoryMock, _logToConsole, _timeServiceMock);
        }


        [TestCase(2, 0)]
        [TestCase(3, 0)]
        [TestCase(5, 0)]
        public async Task ShouldReturnQuoteForRightDate(int currentHour, int dayShift)
        {
            var d = DateTime.Today.AddHours(currentHour);
            var t = DateTime.Today.AddHours(4);
            var f = DateTime.Today.AddHours(2);
            SetupQuote(f, t);
            SetupQuote(f.AddDays(-1), t.AddDays(-1));
            _timeServiceMock.UtcNow.Returns(d);

            var result = await _manager.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().FixingTime.Date, Is.EqualTo(d.Date.AddDays(dayShift)));
        }

    }
}