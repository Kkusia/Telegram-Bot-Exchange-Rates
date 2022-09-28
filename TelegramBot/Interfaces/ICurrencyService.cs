
namespace TelegramBot.Interfaces
{
    public interface ICurrencyService
    {
        public Task<ExchangeRates> GetCurrenciesAsync(DateTime key, Func<DateTime, Task<ExchangeRates>> item);
    }
}
