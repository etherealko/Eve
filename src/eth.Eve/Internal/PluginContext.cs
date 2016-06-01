using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.Storage;

namespace eth.Eve.Internal
{
    internal class PluginContext : IPluginContext
    {
        private readonly PluginInfo _pluginInfo;

        public PluginContext(PluginInfo pluginInfo)
        {
            _pluginInfo = pluginInfo;
        }

        public IPluginLocalStorage GetStorage()
        {
            return new PluginLocalStorage(_pluginInfo);
        }
    }
}
