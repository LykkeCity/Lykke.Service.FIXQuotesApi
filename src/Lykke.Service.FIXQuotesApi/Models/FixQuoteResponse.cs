using System;
using Lykke.Service.FIXQuotesApi.Core.Domain.Models;
using Newtonsoft.Json;

namespace Lykke.Service.FIXQuotesApi.Models
{
    public sealed class FixQuoteResponse
    {
        /// <summary>
        /// An unique asset ID
        /// </summary>
        [JsonProperty("assetPair")]
        public string AssetPair { get; set; }

        /// <summary>
        /// The price calculation time
        /// </summary>
        [JsonProperty("fixingTime")]
        public DateTime FixingTime { get; set; }


        /// <summary>
        /// The time when the trade can be done
        /// </summary>
        [JsonProperty("tradeTime")]
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// The ask price
        /// </summary>
        [JsonProperty("ask")]
        public double Ask { get; set; }

        /// <summary>
        /// The bid price
        /// </summary>
        [JsonProperty("bid")]
        public double Bid { get; set; }

        public static FixQuoteResponse Create(FixQuote fixQuote)
        {
            return new FixQuoteResponse
            {
                AssetPair = fixQuote.AssetPair,
                FixingTime = fixQuote.FixingTime,
                TradeTime = fixQuote.TradeTime,
                Ask = fixQuote.Ask,
                Bid = fixQuote.Bid
            };
        }
    }
}
