using System;

namespace eth.Eve.PluginSystem
{
    public interface IPlugin
    {
        PluginInfo Info { get; }

        void Initialize(IPluginContext ctx);
        void Teardown();

        HandleResult Handle(IUpdateContext msg);
    }
}
