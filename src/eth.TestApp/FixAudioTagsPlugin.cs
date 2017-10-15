using eth.Eve.PluginSystem.BasePlugins;
using System;
using eth.Eve.PluginSystem;

namespace eth.TestApp
{
    public class FixAudioTagsPlugin : PluginBase
    {
        public override PluginInfo Info => new PluginInfo(Guid.Empty,
                                                         "FixAudio",
                                                         "FixAudio",
                                                         "0.0.0.1");

        public override HandleResult Handle(IUpdateContext c)
        {
            var u = c.Update;
            
            if (u.Message?.ReplyToMessage?.Audio == null || !ParseFixCommand(u.Message.Text, out var title, out var performer))
                return HandleResult.Ignored;

            var a = u.Message.ReplyToMessage.Audio;
            
            _ctx.BotApi.SendAudioAsync(chatId: u.Message.Chat.Id, replyToMessageId: u.Message.MessageId,

                audio: a.FileId,
                duration: a.Duration, 

                title: title, 
                performer: performer,
                caption: "ну и говно");

            return HandleResult.HandledCompletely;
        }

        private static bool ParseFixCommand(string text, out string title, out string performer)
        {
            title = performer = null;

            if (text != "говно")
                return false;

            title = "говно";
            performer = "говно2";
            return true;
        }
    }
}
