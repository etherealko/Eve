using eth.Eve.PluginSystem.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eth.Eve.PluginSystem;

namespace eth.TestApp.FancyPlugins
{
    internal class ReplyWithExceptionPlugin : PluginBase, IHealthListener
    {
        public override PluginInfo Info => new PluginInfo(new Guid(), "ReplyWithException", "ReplyWithException", "0.0.0.1");

        public override HandleResult Handle(IUpdateContext c)
        {
            return HandleResult.Ignored;
        }

        public void OnHandleMessageException(Exception ex, IUpdateContext c, IPlugin plugin)
        {
            try
            {
                if (c.IsInitiallyPolled || c.Update.Message?.Chat == null)
                    return;

                var text = $"{plugin}:{Environment.NewLine}{ex}";

                _ctx.BotApi.SendMessageAsync(c.Update.Message.Chat.Id, text, replyToMessageId: c.Update.Message.MessageId);
            }
            catch
            {
            }
        }
    }
}
