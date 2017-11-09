using eth.Telegram.BotApi.Events;

namespace eth.Eve.PluginSystem
{
    public interface IRequestInterceptor
    {
        void OnRequest(RequestEventArgs args);
    }
}
