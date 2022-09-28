using System.Text.Json.Serialization;

namespace TelegramBot
{
    public class ExchangeRate
    {
        [JsonPropertyName("baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("saleRateNB")]
        public double SaleRateNB { get; set; }

        [JsonPropertyName("purchaseRateNB")]
        public double PurchaseRateNB { get; set; }
    }
}
