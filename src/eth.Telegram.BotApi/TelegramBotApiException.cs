using System;
using System.Net;

namespace eth.Telegram.BotApi
{
    public class TelegramBotApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpStatusMessage { get; set; }

        public int? TelegramErrorCode { get; set; }
        public string TelegramDescription { get; set; }
    }
}
