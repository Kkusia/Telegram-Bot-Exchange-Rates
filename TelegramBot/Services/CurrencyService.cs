using Microsoft.Extensions.Caching.Memory;
using TelegramBot.Interfaces;

namespace TelegramBot.Services
{
    public class CurrencyService : ICurrencyService
    {
        private MemoryCache _cache;
        public CurrencyService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        public async Task<ExchangeRates> GetCurrenciesAsync(DateTime key, Func<DateTime, Task<ExchangeRates>> item)
        {
            int secondsToFinish = 30;
            ExchangeRates currencies;
            if (!_cache.TryGetValue(key, out currencies))
            {
                currencies = await item(key);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(secondsToFinish));
                _cache.Set(key, currencies, cacheEntryOptions);
            }
            return currencies;
        }
    }
}
