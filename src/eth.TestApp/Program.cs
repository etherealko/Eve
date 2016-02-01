using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;

namespace eth.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new TelegramBotApi("");

            //ThreadPool.QueueUserWorkItem(o =>
            //{
            //    try
            //    {
            //        var task1 = api.GetMeAsync().Result;
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //});
            //ThreadPool.QueueUserWorkItem(o =>
            //{
            //    try
            //    {
            //        var task1 = api.GetMeAsync().Result;
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //});
            //ThreadPool.QueueUserWorkItem(o =>
            //{
            //    try
            //    {
            //        var task1 = api.GetMeAsync().Result;
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //});

            //Thread.Sleep(5000);

            /////////////////////

            var last = 0;
            List<Update> updates;

            do
            {
                updates = api.GetUpdatesAsync(last, 100, 0).Result;

                if (updates.Count <= 0)
                    break;

                last = updates[updates.Count - 1].UpdateId + 1;

                foreach (var u in updates)
                {
                    if (u.Message?.Sticker != null)
                        ;

                    Console.WriteLine($"{u.Message?.From.FirstName ?? "<no fname>"}: {u.Message.Text}");
                }
            } while (updates.Count > 0);

            Console.WriteLine("==============");

            //var govno = api.GetUpdatesAsync(0, 466577746, 0).Result;
            var dvagovna = api.GetMeAsync().Result;

            for (;;)
            {
                try
                {
                    var msg = Console.ReadLine();

                    if (string.IsNullOrEmpty(msg))
                    {
                        var retS = api.SendSticker(-49047577, "BQADAgADpwADzHD_Ap3b881dw9ADAg");
                        continue;
                    }

                    var ret = api.SendMessageAsync(-49047577, msg).Result;
                }
                catch (AggregateException ex)
                {
                    ex = ex.Flatten();

                    var s = ex.InnerExceptions.SingleOrDefault();
                    if (s != null)
                        throw s;

                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("asshole, here is your exception: " + ex);
                }
            }
        }
    }
}
