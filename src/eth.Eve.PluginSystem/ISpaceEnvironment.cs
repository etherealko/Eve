using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eth.Eve.PluginSystem
{
    public interface ISpaceEnvironment
    {
        IReadOnlyCollection<IPlugin> GetPlugins();

        void AttachPlugin(IPlugin plugin, int position);
        void DetachPlugin(IPlugin plugin);
    }
}
