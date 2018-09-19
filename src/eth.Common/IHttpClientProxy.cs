using System.Net.Http;

namespace eth.Common
{
    public interface IHttpClientProxy
    {
        HttpMessageHandler CreateMessageHandler();
    }
}
