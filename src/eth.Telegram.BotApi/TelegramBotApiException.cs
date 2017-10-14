using System;
using System.Net;

namespace eth.Telegram.BotApi
{
    public class TelegramBotApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; internal set; }
        public string HttpReasonPhrase { get; internal set; }
        public string HttpResponseContent { get; internal set; }
        
        public int? TelegramErrorCode { get; internal set; }
        public string TelegramDescription { get; internal set; }
    }
}
