using System;
using eth.Eve;
using eth.Eve.PluginSystem.BasePlugins;
using System.Threading;
using eth.TestApp.FancyPlugins;
using System.Windows;
using System.IO;
using eth.Eve.PluginSystem;
using eth.Telegram.BotApi.Events;
using eth.Telegram.BotApi;
using System.Linq;

namespace eth.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {            
            //var messageSender = new SimpleMessageSender();
            var sharedStorage = new SimpleSharedStorage();
            var uiSupportPlugin = new UISupportPlugin(sharedStorage);
            var bot = new EveBot();
            
            var spaces = bot.GetSpaceInitializers();

            foreach (var space in spaces)
            {
                space.Value.Plugins.Enqueue(new SimpleConsoleLogger());
                space.Value.Plugins.Enqueue(uiSupportPlugin);
                space.Value.Plugins.Enqueue(new PluginOne());
                //space.Value.Plugins.Enqueue(new PhotoTextPlugin());
                space.Value.Plugins.Enqueue(new LehaTrollerPlugin());
                space.Value.Plugins.Enqueue(new ChannelQuotePlugin());
                //space.Value.Plugins.Enqueue(new FixAudioTagsPlugin());
                space.Value.Plugins.Enqueue(new RampPlugin());
                //space.Value.Plugins.Enqueue(messageSender);
                space.Value.Plugins.Enqueue(sharedStorage);

                space.Value.RequestInterceptors.Enqueue(new Kek());
            }
            
            try
            {
                bot.Start();

                //while (true)
                //{
                //    var txt = Console.ReadLine();
                //    var msg = messageSender.SendTextMessage(-1001013065325, txt).Result;
                //}

                Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("stopping");
                bot.Stop();
                Console.WriteLine("stopped");
            }
            
            Console.ReadLine();
        }
    }

    internal class Kek : IRequestInterceptor
    {
        public void OnRequest(RequestEventArgs args)
        {
            if (args.Method != ApiMethod.SendMessage)
                return;

            var text = args.Arguments.First(a => a.ArgumentName == "text");

            text.Value = (string)text.Value + Environment.NewLine + "лежать+сосать";
        }
    }
}
