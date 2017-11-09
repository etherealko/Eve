using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System.Threading.Tasks;

namespace eth.Eve.PluginSystem
{
    public interface IPluginContext
    {
        ITelegramBotApiWithTimeout BotApi { get; }
        TaskFactory TaskFactory { get; }

        User Me { get; }

        IPluginLocalStorage GetStorage();

        Task<User> GetMeAsync(bool forceServerQuery = false);

        // pipeline
        // bot info, host info
        // 
    }
}