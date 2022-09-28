using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegramBot.Interfaces;
using TelegramBot.Services;

namespace TelegramBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var bot = host.Services.GetRequiredService<IBotService>();
            await bot.StartListeningAsync();
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            services.AddTransient<ICurrencyService, CurrencyService>()
            .AddTransient<IExchangeRateService, ExchangeRateService>()
            .AddTransient<IBotService, BotService>());
        }
    }
}
