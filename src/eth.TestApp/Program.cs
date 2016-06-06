using System;
using eth.Eve;

namespace eth.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bot = new EveBot();

            bot.Start();
            Console.ReadKey(true);
            bot.Stop();
        }
    }
}
