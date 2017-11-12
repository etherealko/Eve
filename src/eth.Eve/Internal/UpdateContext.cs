using eth.Eve.PluginSystem;
using eth.Telegram.BotApi.Objects;

namespace eth.Eve.Internal
{
    internal class UpdateContext : IUpdateContext
    {
        public IPluginContext CurrentContext { get; set; }

        public Update Update { get; set; }
        public bool IsInitiallyPolled { get; set; }

        public object PluginData { get; set; }

    }
}
