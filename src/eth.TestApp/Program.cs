using System;
using eth.Eve;
using System.Threading;
using eth.TestApp.FancyPlugins;
using System.Windows;
using System.IO;
using eth.TestApp.YaDurak;
using eth.PluginSamples;
using Microsoft.EntityFrameworkCore;
using eth.TestApp.FancyPlugins.HogwartsPlugin;

namespace eth.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bot = new EveBot(options => options.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=EveDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"));

            foreach (var space in bot.GetSpaceInitializers())            
                switch (space.Key)
                {
                    case 1:
                        var kek = new Kek();
                        //var messageSender = new SimpleMessageSender();
                        var sharedStorage = new SimpleSharedStorage();
                        var uiSupportPlugin = new UISupportPlugin(sharedStorage);
                        var replyWithExceptionPlugin = new ReplyWithExceptionPlugin();

                        // message handling expected, priority DESC
                        space.Value.Plugins.Enqueue(new SimpleConsoleLogger());
                        space.Value.Plugins.Enqueue(new HealthStatusPlugin());
                        space.Value.Plugins.Enqueue(uiSupportPlugin);
                        space.Value.Plugins.Enqueue(new PluginOne());
                        space.Value.Plugins.Enqueue(new PhotoTextPlugin());
                        space.Value.Plugins.Enqueue(new LehaTrollerPlugin());
                        space.Value.Plugins.Enqueue(new ChannelQuotePlugin());
                        space.Value.Plugins.Enqueue(new FixAudioTagsPlugin());
                        space.Value.Plugins.Enqueue(new RampPlugin());
                        space.Value.Plugins.Enqueue(new HogwartsPlugin());

                        // message handling NOT expected, lowest priority
                        //space.Value.Plugins.Enqueue(messageSender);
                        space.Value.Plugins.Enqueue(sharedStorage);
                        space.Value.Plugins.Enqueue(kek);
                        space.Value.Plugins.Enqueue(replyWithExceptionPlugin);

                        //space.Value.RequestInterceptors.Enqueue(kek);

                        space.Value.ResponseInterceptors.Enqueue(kek);

                        space.Value.HealthListeners.Enqueue(replyWithExceptionPlugin);
                        break;
                    case 2:
                        space.Value.Plugins.Enqueue(new TelegramClientPlugin());
                        space.Value.Plugins.Enqueue(new HealthStatusPlugin());
                        break;
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
}
