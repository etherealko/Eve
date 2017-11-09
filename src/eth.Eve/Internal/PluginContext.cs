using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System.Threading.Tasks;

namespace eth.Eve.Internal
{
    internal class PluginContext : IPluginContext
    {
        private readonly PluginInfo _pluginInfo;
        private readonly EveBotSpace _space;

        public ITelegramBotApi BotApi => _space.OutgoingApi;

        public TaskFactory TaskFactory => _space.TaskFactory;

        public PluginContext(PluginInfo pluginInfo, EveBotSpace space)
        {
            _pluginInfo = pluginInfo;
            _space = space;
        }

        public IPluginLocalStorage GetStorage()
        {
            return new PluginLocalStorage(_pluginInfo, _space.SpaceId);
        }

        public Task<User> GetMe(bool forceServerQuery = false)
        {
            return _space.GetMeAsync(forceServerQuery);
        }
    }
}
