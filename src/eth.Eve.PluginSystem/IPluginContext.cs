using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eth.Eve.PluginSystem
{
    public interface IPluginContext
    {
        ISpaceEnvironment Environment { get; }

        ITelegramBotApiWithTimeout BotApi { get; }
        TaskFactory TaskFactory { get; }

        User Me { get; }

        object PluginData { get; set; }

        IPluginLocalStorage GetStorage();

        Task<User> GetMeAsync(bool forceServerQuery = false);

        Task Run(Action action);
        Task<T> Run<T>(Func<T> action);
    }
}