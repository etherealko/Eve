using System;

namespace eth.Eve.PluginSystem
{
    public interface IPlugin
    {
        PluginInfo Info { get; }

        void Initialize(IPluginContext ctx);
        void Initialized();
        void Teardown();

        HandleResult Handle(IUpdateContext c);
    }
}
