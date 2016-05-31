using eth.Telegram.BotApi.Objects;

namespace eth.Eve.PluginSystem
{
    public interface IMessageContext
    {
        Update Update { get; }

        bool IsInitiallyPolled { get; }
    }
}