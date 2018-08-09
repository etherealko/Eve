using System;
using System.Net.Http;
using eth.Common;

namespace eth.Telegram.BotApi.Proxies
{
    public class HttpProxy : IHttpClientProxy
    {
        public HttpMessageHandler CreateMessageHandler()
        {
            throw new NotImplementedException();
        }
    }
}
