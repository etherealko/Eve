using System;
using eth.Eve;

namespace eth.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bot = new EveBot();

            var spaces = bot.GetSpaceInitializers();

            foreach (var space in spaces)
                space.Value.Plugins.Enqueue(new PluginOne());

            bot.Start();
            Console.ReadKey(true);
            bot.Stop();
        }
    }
}
