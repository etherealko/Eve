using eth.Eve.PluginSystem.BasePlugins;
using System;
using eth.Eve.PluginSystem;
using System.Threading;

namespace eth.TestApp
{
    public class UISupportPlugin : PluginBase, IDisposable
    {
        private Thread _thread;

        public SimpleSharedStorage SharedStorage { get; }

        public override PluginInfo Info => new PluginInfo(new Guid("fbc46365-dc78-4f6d-8d7f-2ad1822138c0"), "UISupportPlugin", "UISupportPlugin", "0.0.0.1");

        public long ChatId { get; private set; }

        public IPluginContext PluginContext => _ctx;
        
        public UISupportPlugin(SimpleSharedStorage sharedStorage)
        {
            SharedStorage = sharedStorage;
        }

        public override void Initialized()
        {
            if (!SharedStorage.GetStorage().TryGetString("govnoChat", out var govnoChat))
                throw new Exception();

            ChatId = long.Parse(govnoChat.Value);
            
            _thread = new Thread(UIMain);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start(this);
        }

        public override HandleResult Handle(IUpdateContext c)
        {
            return HandleResult.Ignored;
        }

        public override void Teardown()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_thread != null)
                _thread.Abort();
        }

        private static void UIMain(object uiSupportPlugin)
        {
            var wnd = new MainWindow((UISupportPlugin)uiSupportPlugin);
            wnd.ShowDialog();
        }
    }
}
