
namespace TelegramBot.Interfaces
{
    public interface IExchangeRateService
    {
        public Task<string> GetExchangeRateAsync(string message);
    }
}
