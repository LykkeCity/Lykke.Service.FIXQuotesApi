using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using NUnit.Framework;

namespace Lykke.Service.FIXQuotesApi.Tests
{
    [TestFixture]
    internal sealed class InputModelContractTest
    {
        private const string InputMode = "[{\"AssetPair\":\"ELECBLOOUSD\",\"FixingTime\":\"2017-09-15T09:41:00Z\",\"TradeTime\":\"2017-09-15T16:00:00Z\",\"Ask\":1.01,\"Bid\":0.9801},{\"AssetPair\":\"ICOEUR\",\"FixingTime\":\"2017-09-15T09:41:00Z\",\"TradeTime\":\"2017-09-15T16:00:00Z\",\"Ask\":0.8459861,\"Bid\":0.8292438}]";

        [Test]
        public void ShouldDeserializeJson()
        {
            var des = new JsonMessageDeserializer<IReadOnlyCollection<FixQuote>>();
            var stream = Encoding.ASCII.GetBytes(InputMode);
            var model = des.Deserialize(stream);
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(2));
            Assert.That(model.First().AssetPair,Is.EqualTo("ELECBLOOUSD"));
        }
    }
}
