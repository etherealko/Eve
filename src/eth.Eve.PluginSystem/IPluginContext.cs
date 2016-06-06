using eth.Eve.PluginSystem.Storage;
using eth.Telegram.BotApi;

namespace eth.Eve.PluginSystem
{
    public interface IPluginContext
    {
        ITelegramBotApi BotApi { get; }

        IPluginLocalStorage GetStorage();

        // pipeline
        // bot info, host info
        // 
    }
}