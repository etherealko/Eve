using System;

namespace eth.Eve.PluginSystem
{
    public interface IPlugin : IDisposable
    {
        PluginInfo Info { get; }

        void Initialize(IPluginContext ctx);
        void Teardown();

        HandleResult Handle(IMessageContext msg);
    }
}
