using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;

namespace eth.Eve.Internal
{
    internal class PluginContext : IPluginContext
    {
        private readonly PluginInfo _pluginInfo;
        private readonly long _spaceId;

        public ITelegramBotApi BotApi { get; }

        public PluginContext(PluginInfo pluginInfo, long spaceId, ITelegramBotApi botApi)
        {
            _pluginInfo = pluginInfo;
            _spaceId = spaceId;
            BotApi = botApi;
        }

        public IPluginLocalStorage GetStorage()
        {
            return new PluginLocalStorage(_pluginInfo, _spaceId);
        }
    }
}
