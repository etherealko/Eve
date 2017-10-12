using eth.Eve.PluginSystem;
using System.Collections.Generic;

namespace eth.Eve
{
    public interface IEveSpaceInitializer
    {
        long SpaceId { get; }

        Queue<IPlugin> Plugins { get; }
    }
}
