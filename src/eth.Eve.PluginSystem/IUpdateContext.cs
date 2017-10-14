using eth.Telegram.BotApi.Objects;

namespace eth.Eve.PluginSystem
{
    public interface IUpdateContext
    {
        Update Update { get; }

        bool IsInitiallyPolled { get; }

        object PluginData { get; set; }
    }
}