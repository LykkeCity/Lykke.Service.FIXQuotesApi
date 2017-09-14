using System;
using Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.FIXQuotesApi.AzureRepositories
{
    public sealed class FixQuoteEntity : TableEntity
    {
        public string Id { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }
        public DateTime FixingTime { get; set; }
        public DateTime TradeTime { get; set; }

        public FixQuoteEntity()
        {

        }

        public static string ToPartitionKey(DateTime fixingTime)
        {
            return fixingTime.ToIsoDate();
        }

        public static string ToRowKey(string id)
        {
            return id;
        }

        public FixQuoteEntity(string id, double ask, double bid, DateTime fixingTime, DateTime tradeTime)
        {
            PartitionKey = ToPartitionKey(fixingTime);
            RowKey = ToRowKey(id);
            Id = id;
            Ask = ask;
            Bid = bid;
            FixingTime = fixingTime;
            TradeTime = tradeTime;
        }
    }
}