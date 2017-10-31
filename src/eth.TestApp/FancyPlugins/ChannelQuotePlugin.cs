using eth.Eve.PluginSystem;
using eth.Eve.PluginSystem.BasePlugins;
using System;
using System.Text.RegularExpressions;

namespace eth.TestApp
{
    public class ChannelQuotePlugin : PluginBase
    {
        private const long ChatId = -1001013065325;
        private const int AdminId = 108762758;

        private const long ChannelId = -1001024213089;

        public override PluginInfo Info => new PluginInfo(new Guid("871e3e91-d372-4367-91e3-ae30dfd80c0d"),
                                                         "ChannelQuote",
                                                         "Quote it",
                                                         "0.0.0.1");
        
        public override HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled || c.Update?.Message?.Chat.Id != ChatId)
                return HandleResult.Ignored;

            var msg = c.Update.Message;
            
            if (msg?.ReplyToMessage != null &&
                c.Update.Message.From.Id == AdminId && 
                Regex.IsMatch(c.Update.Message.Text ?? "", "#говно", RegexOptions.IgnoreCase))
            {
                _ctx.BotApi.ForwardMessageAsync(ChannelId, ChatId, msg.ReplyToMessage.MessageId);
            }
            
            return HandleResult.Ignored;
        }
    }
}
