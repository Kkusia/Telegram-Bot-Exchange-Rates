using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot;
using TelegramBot.Interfaces;
using TelegramBot.Services;
using Moq;

namespace TelegramBotTests
{
    [TestClass]
    public class BotTests
    {
        [TestMethod]
        public void StartMessegeTest()
        {
            var service = new Mock<IExchangeRateService>();
            var botClient = new TelegramBotClient(BotService.Token);
            var cts = new CancellationToken();
            var update = new Update();
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                message.Text = "/start";
                var result = new BotService(service.Object).HandleUpdateAsync(botClient, update, cts);
                var actual = "Welcome to ExchangerBot! Please, enter currency and date to get result.";
                Assert.AreEqual(actual, result);
            }
        }
        [TestMethod]
        public void WrongInputTest()
        {
            var service = new Mock<ICurrencyService>();
            ExchangeRateService rateService = new ExchangeRateService(service.Object);
            var message = "BVGH 54.23.1900";
            var result = rateService.GetExchangeRateAsync(message).Result;
            var actual = "Wrong input. Please, enter the correct currency code and date 'dd.mm.yyyy'";
            Assert.AreEqual(actual, result);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]  
        public void WrongInputCurrencyCodeTest()
        {
            var service = new Mock<ICurrencyService>();
            ExchangeRateService rateService = new ExchangeRateService(service.Object);
            var exchangeRates = new ExchangeRates();
            var currency = new ExchangeRate
            {
                Currency = "utr"
            };
            var message = $"{currency}";
            var result = rateService.GetRequestedCurrency(exchangeRates, message);
            var actual = "Bot doesn't have information about asked currency rate at specified date. " +
                "Please, check whether the currency is correct.";
            Assert.AreEqual(actual, result);
        }
    }
}