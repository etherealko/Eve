using eth.Eve.PluginSystem.Storage;

namespace eth.Eve.PluginSystem
{
    public interface IPluginContext
    {
        IPluginLocalStorage GetStorage();

        // pipeline
        // bot info, host info
        // 
    }
}