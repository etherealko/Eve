using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System;
using System.Threading.Tasks;

namespace eth.Eve.Internal
{
    internal class PluginContext : IPluginContext
    {
        private readonly EveBotSpace _space;

        ITelegramBotApiWithTimeout IPluginContext.BotApi => BotApi;

        public TelegramBotApi BotApi { get; }
        public IPlugin Plugin { get; }

        public TaskFactory TaskFactory => _space.TaskFactory;

        public User Me => _space.GetMeAsync(false).Result;

        public PluginContext(IPlugin plugin, EveBotSpace space, TelegramBotApi outgoingApi)
        {
            Plugin = plugin;
            BotApi = outgoingApi;
            _space = space;
        }

        public IPluginLocalStorage GetStorage()
        {
            return new PluginLocalStorage(Plugin.Info, _space.SpaceId);
        }

        public Task<User> GetMeAsync(bool forceServerQuery = false)
        {
            return _space.GetMeAsync(forceServerQuery);
        }
    }
}
