using eth.Eve.PluginSystem;
using eth.Telegram.BotApi.Objects;

namespace eth.Eve.Internal
{
    internal class MessageContext : IMessageContext
    {
        public Update Update { get; set; }
        public bool IsInitiallyPolled { get; set; }
    }
}
