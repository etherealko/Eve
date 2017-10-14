using System;
using eth.Eve;
using eth.Eve.PluginSystem.BasePlugins;

namespace eth.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var messageSender = new SimpleMessageSender();
            var bot = new EveBot();

            var spaces = bot.GetSpaceInitializers();

            foreach (var space in spaces)
            {
                space.Value.Plugins.Enqueue(new SimpleConsoleLogger());
                space.Value.Plugins.Enqueue(new PluginOne());
                space.Value.Plugins.Enqueue(messageSender);
            }
            
            try
            {
                bot.Start();
                
                while (true)
                {
                    var txt = Console.ReadLine();
                    var msg = messageSender.SendTextMessage(108762758, txt).Result;
                }
            }
            finally
            {
                bot.Stop();
            }
        }
    }
}
