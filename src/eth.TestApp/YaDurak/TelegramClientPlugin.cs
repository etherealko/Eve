using eth.Eve.PluginSystem.BasePlugins;
using System;
using eth.Eve.PluginSystem;
using System.Threading;

namespace eth.TestApp.YaDurak
{
    public class TelegramClientPlugin : PluginBase, IDisposable
    {
        private Thread _thread;

        public override PluginInfo Info => new PluginInfo(new Guid("D20C8C66-F494-436B-8B52-A72CA22CA4CC"), "TelegramClient", "TelegramClient", "0.0.0.1");
        
        public IPluginContext PluginContext => _ctx;
        
        public TelegramClientPlugin()
        {
        }

        public override void Initialized()
        {            
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

        private static void UIMain(object telegramClientPlugin)
        {
            var wnd = new TelegramClientWindow((TelegramClientPlugin)telegramClientPlugin);
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
}
