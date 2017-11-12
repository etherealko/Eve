using eth.Eve.PluginSystem.Storage;
using System;

namespace eth.Eve.PluginSystem.Samples
{
    public class SimpleSharedStorage : PluginBase
    {
        private readonly Guid _pluginGuid = new Guid("f077c67a-c019-418f-8ee3-b77c7314be57");

        public override PluginInfo Info { get; }

        public SimpleSharedStorage()
        {
            Info = new PluginInfo(_pluginGuid, "SimpleSharedStorage", "", "0.0.0.1");
        }

        public SimpleSharedStorage(Guid pluginGuidOverride) : this()
        {
            _pluginGuid = pluginGuidOverride;
        }

        public override HandleResult Handle(IUpdateContext c)
        {
            return HandleResult.Ignored;
        }

        public IPluginLocalStorage GetStorage()
        {
            return _ctx.GetStorage();
        }
    }
}
