using eth.Telegram.BotApi.Events;

namespace eth.Eve.PluginSystem
{
    public interface IResponseInterceptor
    {
        void OnResponse(ResponseEventArgs args);
    }
}
