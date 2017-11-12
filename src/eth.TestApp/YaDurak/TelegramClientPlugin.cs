using eth.Eve.PluginSystem.BasePlugins;
using System;
using eth.Eve.PluginSystem;
using System.Threading;

namespace eth.TestApp.YaDurak
{
    public class TelegramClientPlugin : PluginBase, IDisposable
    {
        private Thread _thread;
        private ManualResetEventSlim _wndCreated = new ManualResetEventSlim();

        public override PluginInfo Info => new PluginInfo(new Guid("D20C8C66-F494-436B-8B52-A72CA22CA4CC"), "TelegramClient", "TelegramClient", "0.0.0.1");
        
        public IPluginContext PluginContext => _ctx;

        public event EventHandler<HandleEventArgs> HandleEvent;
        
        public override void Initialize(IPluginContext ctx)
        {
            base.Initialize(ctx);
            
            _thread = new Thread(UIMain);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start(this);
            _wndCreated.Wait();
        }

        public override HandleResult Handle(IUpdateContext c)
        {
            var e = HandleEvent;

            if (e == null)
                return HandleResult.Ignored;
            
            e(this, new HandleEventArgs { UpdateContext = c });
            return HandleResult.HandledPartially;
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

        private void UIMain()
        {
            var wnd = new TelegramClientWindow(this);
            _wndCreated.Set();

            try
            {
                wnd.ShowDialog();
            }
            catch
            {
                wnd.Close();
            }
        }
    }

    public class HandleEventArgs
    {
        public IUpdateContext UpdateContext { get; set; }
    }
}
