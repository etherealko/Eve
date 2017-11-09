using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System.Threading.Tasks;

namespace eth.Eve.PluginSystem
{
    public interface IPluginContext
    {
        ITelegramBotApi BotApi { get; }
        TaskFactory TaskFactory { get; }

        IPluginLocalStorage GetStorage();

        Task<User> GetMe(bool forceServerQuery = false);

        // pipeline
        // bot info, host info
        // 
    }
}