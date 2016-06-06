using System;
using eth.Eve.PluginSystem;

namespace eth.TestApp
{
    public class PluginOne : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid(""), 
                                                         "PluginOne", 
                                                         "Brand new Eve plugin", 
                                                         "0.0.0.1");

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Teardown()
        {
            Dispose();
        }

        public HandleResult Handle(IMessageContext msg)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }
    }
}
