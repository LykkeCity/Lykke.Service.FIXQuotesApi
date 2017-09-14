// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.FIXQuotesApi.Client.AutorestClient.Models
{
    using Newtonsoft.Json;

    public partial class FixQuoteModel
    {
        /// <summary>
        /// Initializes a new instance of the FixQuoteModel class.
        /// </summary>
        public FixQuoteModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FixQuoteModel class.
        /// </summary>
        public FixQuoteModel(System.DateTime fixingTime, System.DateTime tradeTime, double ask, double bid, string assetPair = default(string))
        {
            AssetPair = assetPair;
            FixingTime = fixingTime;
            TradeTime = tradeTime;
            Ask = ask;
            Bid = bid;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPair")]
        public string AssetPair { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FixingTime")]
        public System.DateTime FixingTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TradeTime")]
        public System.DateTime TradeTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Ask")]
        public double Ask { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Bid")]
        public double Bid { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
