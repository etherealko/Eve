using eth.Eve.PluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eth.TestApp
{
    public class RampPlugin : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("0e1cfecf-c763-48ea-bad2-57f9cbf4e1ba"),
                                                         "RampPlugin",
                                                         "The Ramp 3.0 Eve Plugin",
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
