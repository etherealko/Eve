using eth.Eve.PluginSystem.BasePlugins;
using System;
using eth.Eve.PluginSystem;
using System.Threading.Tasks;
using eth.Telegram.BotApi;
using System.IO;
using eth.Telegram.BotApi.Objects.Enums;
using System.Linq;
using eth.Telegram.BotApi.Objects;

#pragma warning disable CS4014 //missing await

namespace eth.TestApp.FancyPlugins
{
    public class FixAudioTagsPlugin : PluginBase
    {
        public override PluginInfo Info => new PluginInfo(Guid.Empty,
                                                         "FixAudio",
                                                         "FixAudio",
                                                         "0.0.0.1");
        
        public override HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
                return HandleResult.Ignored;

            var u = c.Update;

            Audio audio;

            if ((audio = u.Message.Audio) == null && (audio = u.Message?.ReplyToMessage?.Audio) == null)
                return HandleResult.Ignored;

            if (!ParseFixCommand(u.Message.Text ?? u.Message.Caption, out var title, out var performer))
                return HandleResult.Ignored;
            
            Task.Run(async () =>
            {
                _ctx.BotApi.SendChatActionAsync(u.Message.Chat.Id, ChatAction.UploadingDocument);

                var fileInfo = await _ctx.BotApi.GetFileInfoAsync(audio.FileId);
                var fileBytes = await _ctx.BotApi.GetFileBytesAsync(fileInfo.FilePath);
                var fileStream = new MemoryStream(fileBytes);
                
                var r = await _ctx.BotApi.SendAudioAsync(chatId: u.Message.Chat.Id, replyToMessageId: u.Message.MessageId,
                    audio: new InputFile(fileStream, Path.GetFileName(fileInfo.FilePath)),
                    duration: audio.Duration,

                    title: title,
                    performer: performer,
                    caption: null);
            });

            return HandleResult.HandledCompletely;
        }

        private static bool ParseFixCommand(string text, out string title, out string performer)
        {
            title = performer = null;
            
            if (text == null || !text.StartsWith("tags ", StringComparison.OrdinalIgnoreCase))
                return false;

            var args = text.Substring(4).Split('-', '–').Select(s => s.Trim()).ToArray();

            if (args.Length != 2)
                return false;

            performer = args[0];
            title = args[1];
            return true;
        }
    }
}
