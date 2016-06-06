using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;

namespace eth.Eve.Internal
{
    internal class PluginContext : IPluginContext
    {
        private readonly PluginInfo _pluginInfo;

        public ITelegramBotApi BotApi { get; }

        public PluginContext(PluginInfo pluginInfo, ITelegramBotApi botApi)
        {
            _pluginInfo = pluginInfo;
            BotApi = botApi;
        }

        public IPluginLocalStorage GetStorage()
        {
            return new PluginLocalStorage(_pluginInfo);
        }
    }
}
