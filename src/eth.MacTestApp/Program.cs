﻿using System;
using eth.Eve;
using eth.PluginSamples;
using eth.TestApp.FancyPlugins;
using eth.Eve.Storage;
using Microsoft.EntityFrameworkCore;
using eth.Telegram.BotApi.Objects;

namespace eth.MacTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new EveBot(builder => builder.UseMySql(@"server=localhost;port=3306;database=eveDb;uid=root;password=password"));

            var messageSender = new SimpleMessageSender();

            foreach (var space in bot.GetSpaceInitializers())
                switch (space.Key)
                {
                    case 1:
                    var kek = new Kek();
                        var replyWithExceptionPlugin = new ReplyWithExceptionPlugin();

                        // message handling expected, priority DESC
                        space.Value.Plugins.Enqueue(new SimpleConsoleLogger());
                        space.Value.Plugins.Enqueue(new HealthStatusPlugin());
                        space.Value.Plugins.Enqueue(new PluginOne());
                        space.Value.Plugins.Enqueue(new LehaTrollerPlugin());
                        space.Value.Plugins.Enqueue(new ChannelQuotePlugin());
                        space.Value.Plugins.Enqueue(new FixAudioTagsPlugin());
                        space.Value.Plugins.Enqueue(new RampPlugin());

                        // message handling NOT expected, lowest priority
                        space.Value.Plugins.Enqueue(messageSender);
                        space.Value.Plugins.Enqueue(kek);
                        space.Value.Plugins.Enqueue(replyWithExceptionPlugin);

                        //space.Value.RequestInterceptors.Enqueue(kek);

                        space.Value.ResponseInterceptors.Enqueue(kek);

                        space.Value.HealthListeners.Enqueue(replyWithExceptionPlugin);
                        break;
                    default:
                        space.Value.Disable();
                        break;
                }

            try
            {
                bot.Start();

                while (true)
                {
                    var txt = Console.ReadLine();

                    if (txt == "")
                        break;

                    var msg = messageSender.SendTextMessage(-1001013065325, txt).Result;
                }

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
