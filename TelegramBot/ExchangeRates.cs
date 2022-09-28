using System.Text.Json.Serialization;

namespace TelegramBot
{
    public class ExchangeRates
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("exchangeRate")]
        public List<ExchangeRate> CurrencyRates { get; set; }
    }
}
