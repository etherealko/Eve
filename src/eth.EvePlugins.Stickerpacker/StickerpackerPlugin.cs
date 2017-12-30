using eth.Eve.PluginSystem;
using System;

namespace eth.PluginSamples
{
    public sealed class StickerpackerPlugin : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("AA3EC3F1-7864-489C-8334-3D36E1BA9A49"), "Stickerpacker", "Stickerpacker", "0.0.0.1");

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialized() { }

        public HandleResult Handle(IUpdateContext c)
        {
            throw new NotImplementedException();
        }

        public void Teardown() { }
    }
}