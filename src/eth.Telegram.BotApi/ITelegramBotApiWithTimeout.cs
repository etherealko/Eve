using eth.Common;

namespace eth.Telegram.BotApi
{
    public interface ITelegramBotApiWithTimeout : ITelegramBotApi, IHttpClientTimeout { }
}
