using System;
using eth.Eve;
using eth.Eve.PluginSystem.BasePlugins;
using System.Threading;
using eth.TestApp.FancyPlugins;
using System.Windows;
using System.IO;

namespace eth.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {            
            var messageSender = new SimpleMessageSender();
            var sharedStorage = new SimpleSharedStorage();
            var uiSupportPlugin = new UISupportPlugin(sharedStorage);
            var bot = new EveBot();
            
            var spaces = bot.GetSpaceInitializers();

            foreach (var space in spaces)
            {
                space.Value.Plugins.Enqueue(new SimpleConsoleLogger());
                space.Value.Plugins.Enqueue(uiSupportPlugin);
                space.Value.Plugins.Enqueue(new PluginOne());
                space.Value.Plugins.Enqueue(new PhotoTextPlugin());
                space.Value.Plugins.Enqueue(new LehaTrollerPlugin());
                space.Value.Plugins.Enqueue(new ChannelQuotePlugin());
                space.Value.Plugins.Enqueue(new FixAudioTagsPlugin());
                space.Value.Plugins.Enqueue(new RampPlugin());
                space.Value.Plugins.Enqueue(messageSender);
                space.Value.Plugins.Enqueue(sharedStorage);
            }
            
            try
            {
                bot.Start();

                var thread = new Thread(UIMain);
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start(uiSupportPlugin);

                while (true)
                {
                    var txt = Console.ReadLine();
                    var msg = messageSender.SendTextMessage(-1001013065325, txt).Result;
                }
            }
            finally
            {
                bot.Stop();
            }
        }

        private static void UIMain(object uiSupportPlugin)
        {
            //var wnd1 = new Window();
            //wnd1.Content = new PhotoTextRenderControl(File.ReadAllBytes(@"C:\Users\eth\Desktop\mgsvtpp 2017-10-16 14-14-07-35.jpg"), "govno kakoe-to");
            //wnd1.ShowDialog();

            var wnd = new MainWindow((UISupportPlugin)uiSupportPlugin);
            wnd.ShowDialog();
        }
    }
}
