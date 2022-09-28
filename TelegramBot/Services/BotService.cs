using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramBot.Interfaces;
using Telegram.Bot.Extensions.Polling;

namespace TelegramBot.Services
{
    public class BotService : IBotService
    {
        public static string Token { get; set; } = "5737523071:AAGxeKPcLylwLzdNLmqCBjIvgVf_xyI4BEs";
        private static ITelegramBotClient bot = new TelegramBotClient(Token);
        private IExchangeRateService _service;
        public BotService(IExchangeRateService service)
        {
            _service = service;
        }
        public async Task StartListeningAsync()
        {
            Console.WriteLine("Start ExchangerBot " + bot.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cts)
        {
            if(update.Type == UpdateType.Message)
            {
                var messg = update.Message;
                if(messg.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(messg.Chat, "Welcome to ExchangerBot! " +
                        "Please, enter currency and date to get result.");
                    return;
                }
            }
            if (update.Type != UpdateType.Message)
            {
                return;
            }
            if (update.Message!.Type != MessageType.Text)
            {
                return;
            }
            var chatId = update.Message.Chat.Id;
            var message = update.Message.Text;
            var answer = await _service.GetExchangeRateAsync(message);
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: answer,
                cancellationToken: cts);
        }
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cts)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
