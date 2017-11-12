using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects;
using System;
using System.Threading.Tasks;
using eth.Eve.PluginSystem;

namespace eth.PluginSamples
{
    public class SimpleMessageSender : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("51b9ab14-7eab-4107-83bf-c9e50a2824f4"),
                                                         "PluginOne",
                                                         "Brand new Eve plugin",
                                                         "0.0.0.1");

        public Task<Message> SendTextMessage(ChatIdOrUsername chatId, string message)
        {
            return _ctx.BotApi.SendMessageAsync(chatId, message);
        }

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialized() { }

        public void Teardown()
        {
            Dispose();
        }

        public HandleResult Handle(IUpdateContext msg)
        {
            return HandleResult.Ignored;
        }

        public void Dispose() { }
    }
}
