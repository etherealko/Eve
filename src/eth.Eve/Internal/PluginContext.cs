using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eth.Eve.Internal
{
    internal class PluginContext : IPluginContext, IDisposable
    {
        private readonly EveBotSpace _space;

        private int _disposed;

        ITelegramBotApiWithTimeout IPluginContext.BotApi => BotApi;

        public TelegramBotApi BotApi { get; }
        public IPlugin Plugin { get; }
        
        public ISpaceEnvironment Environment => throw new NotImplementedException();

        public TaskFactory TaskFactory => _space.TaskFactory;

        public User Me => _space.GetMeAsync(false).Result;

        public object PluginData { get; set; }

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

        public Task Run(Action action)
        {
            return TaskFactory.StartNew(action);
        }

        public Task<T> Run<T>(Func<T> action)
        {
            return TaskFactory.StartNew(action);
        }

        public void Dispose()
        {
            var disposed = Interlocked.Exchange(ref _disposed, 1);

            if (disposed == 1)
                return;

            Plugin.Teardown();
            BotApi.Dispose();
        }
    }
}
