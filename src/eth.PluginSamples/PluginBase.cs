using eth.Eve.PluginSystem;

namespace eth.PluginSamples
{
    public abstract class PluginBase : IPlugin
    {
        protected IPluginContext _ctx;

        public abstract PluginInfo Info { get; }

        public abstract HandleResult Handle(IUpdateContext c);

        public virtual void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public virtual void Initialized() { }

        public virtual void Teardown() { }
    }
}
