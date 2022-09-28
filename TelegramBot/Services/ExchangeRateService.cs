using System.Text.Json;
using TelegramBot.Interfaces;

namespace TelegramBot.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private ICurrencyService _currencyService;
        private HttpClient _client;
        public ExchangeRateService(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _client = new HttpClient();
        }
        public async Task<string> GetExchangeRateAsync(string message)
        {
            try
            {
                IsCorrectInput(message, out var currencyCode, out var date);
                var currencyRates = await _currencyService.GetCurrenciesAsync(date, GetUAHRatesOnDateAsync);
                var currencyRate = GetRequestedCurrency(currencyRates, currencyCode);
                var result = GetResultExchangeRate(currencyRate);
                return $"{date.ToShortDateString()}: 1 {currencyCode} = {result} UAH";
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return ex.Message;
            }
        }
        private bool IsCorrectInput(string message, out string currencyCode, out DateTime date)
        {
            var strings = message.Split(' ');
            if (strings.Length >= 2 && DateTime.TryParse(strings[1], out date))
            {
                currencyCode = strings[0].ToUpper();
                return true;
            }
            throw new Exception("Wrong input. Please, enter the correct currency code and date 'dd.mm.yyyy'");
        }
        public ExchangeRate GetRequestedCurrency(ExchangeRates currencyRates, string currencyCode)
        {
            var currencyRate = currencyRates
                .CurrencyRates
                .Where(x => x.Currency == currencyCode)
                .FirstOrDefault();
            if (currencyRate == null)
            {
                throw new ArgumentNullException("Bot doesn't have information about asked currency rate at " +
                    "specified date. Please, check whether the currency is correct.");
            }
            return currencyRate;
        }
        private async Task<ExchangeRates> GetUAHRatesOnDateAsync(DateTime date)
        {
            var stringDate = $"&date={date.Day}.{date.Month}.{date.Year}";
            var uri = new Uri("https://api.privatbank.ua/p24api/exchange_rates?json" + stringDate);
            var streamTask = _client.GetStreamAsync(uri);
            return await JsonSerializer.DeserializeAsync<ExchangeRates>(await streamTask);
        }
        private double GetResultExchangeRate(ExchangeRate rate)
        {
            return (rate.PurchaseRateNB + rate.SaleRateNB) / 2;
        }
    }
}
