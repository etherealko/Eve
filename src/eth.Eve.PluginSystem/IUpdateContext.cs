using eth.Telegram.BotApi.Objects;

namespace eth.Eve.PluginSystem
{
    public interface IUpdateContext
    {
        IPluginContext CurrentContext { get; }

        Update Update { get; }
        bool IsInitiallyPolled { get; }

        object PluginData { get; set; }
    }
}