using eth.Eve.PluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eth.TestApp
{
    public class RampPlugin : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("0e1cfecf-c763-48ea-bad2-57f9cbf4e1ba"),
                                                         "RampPlugin",
                                                         "The Ramp 3.0 Eve Plugin",
                                                         "0.0.0.1");

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Teardown()
        {
            Dispose();
        }

        public HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
                return HandleResult.Ignored;

            switch (c.Update.Message?.Text)
            {
                case "/stuff":
                case "/stuff@ramp30_bot":
                case "/nishebrod":
                case "/nishebrod@ramp30_bot":
                case "/bankroll":
                case "/bankroll@ramp30_bot":
                    var msg = c.Update.Message;

                    try
                    {
                        throw new NotImplementedException();
                    }
                    catch (Exception ex)
                    {
                        var exFirstLine = ex.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0];
                        var fullStackTrace = Environment.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                        var msgBuilder = new StringBuilder(exFirstLine + Environment.NewLine);

                        for (var i = 2; i < fullStackTrace.Length; ++i)                        
                            msgBuilder.AppendLine(fullStackTrace[i]);

                        var output = msgBuilder.ToString().Substring(0, 4000);

                        _ctx.BotApi.SendMessageAsync(chatId: msg.Chat.Id, replyToMessageId: msg.MessageId, text: output);
                    }

                    return HandleResult.HandledCompletely;
                default:
                    return HandleResult.Ignored;
            }
        }

        public void Dispose() { }
    }
}
