using System;
using System.Text.RegularExpressions;
using eth.Eve.PluginSystem;
using eth.Telegram.BotApi.Objects.Enums;
using eth.Telegram.BotApi.Objects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Linq;
using eth.Telegram.BotApi;
using Newtonsoft.Json;

#pragma warning disable CS4014 //missing await

namespace eth.TestApp
{
    internal class PluginOne : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("51b9ab14-7eab-4107-83bf-c9e50a2824f4"), 
                                                         "PluginOne", 
                                                         "Brand new Eve plugin", 
                                                         "0.0.0.1");

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;

            var chat = _ctx.BotApi.GetMeAsync().Result;
        }

        public void Initialized() { }

        public void Teardown()
        {
            
        }

        public HandleResult Handle(IUpdateContext msg)
        {
            if (msg.IsInitiallyPolled)
                return HandleResult.Ignored;

            var update = msg.Update;

            if (update.Message == null)
                return HandleResult.Ignored;

            if (update.Message.Chat.Id == -1001013065325)
            {
                var eva = new Regex(@"\bmiu\b", RegexOptions.IgnoreCase);

                if (eva.IsMatch(update.Message.Text ?? ""))
                {
                    _ctx.BotApi.SendMessageAsync(-1001013065325, "i'm here", ParseMode.None, null, null, update.Message.MessageId);
                    return HandleResult.HandledCompletely;
                }

                if (update.Message.From.Id == 146268050)
                {
                    var roll = new Random().Next(10);
                    if (roll == 7)
                        _ctx.BotApi.SendMessageAsync(-1001013065325, "юль, ну впиши по-братски");
                    return HandleResult.HandledCompletely;
                }
            }

            if (update.Message?.Text == "get" && update.Message?.ReplyToMessage?.Audio != null)
            {
                Task.Factory.StartNew(async () => 
                {
                    var audio = update.Message?.ReplyToMessage?.Audio;
                    var fileInfo = await _ctx.BotApi.GetFileInfoAsync(audio.FileId);

                    using (var fileStream = await _ctx.BotApi.GetFileStreamAsync(fileInfo.FilePath))
                    {
                        var ms = new MemoryStream();
                        fileStream.CopyTo(ms);
                                                
                        var bytes = ms.ToArray();
                    }
                }, TaskCreationOptions.LongRunning);
                
                return HandleResult.HandledCompletely;
            }

            return HandleResult.Ignored;
        }
    }
}
