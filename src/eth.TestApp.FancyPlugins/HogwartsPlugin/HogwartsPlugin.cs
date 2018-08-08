using eth.Eve.PluginSystem;
using System;
using System.Linq;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsPlugin : IPlugin
    {
        private bool AllowPrivateChat = false;
        private bool RestrictOtherConf = true;
        private long ConfId = -1001013065325;

        private HogwartsService Service { get; set; }
        
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("4d9b36d0-ec43-4270-89f2-262cf05f1814"),
                                                         "HogwartsPlugin",
                                                         "Hogwarts Plugin",
                                                         "0.0.0.1");

        public HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
            {
                return HandleResult.Ignored;
            }

            var msg = c.Update.Message;
            if (msg == null || msg.Text == null)
            {
                return HandleResult.Ignored;
            }

            if (!AllowPrivateChat && msg.Chat.Type == Telegram.BotApi.Objects.Enums.ChatType.Private)
            {
                return HandleResult.Ignored;
            }

            var chatId = msg.Chat.Id;
            if (RestrictOtherConf && chatId != ConfId)
            {
                return HandleResult.Ignored;
            }

            if (Service.HandleMessage(msg))
            {
                var response = Service.PendingResponse?.First();
                if (response != null)
                {
                    _ctx.BotApi.SendMessageAsync(chatId, response);
                }
                return HandleResult.HandledCompletely;
            }
            else
            {
                return HandleResult.Ignored;
            }
        }

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
            var storage = _ctx.GetStorage();
            Service = new HogwartsService(storage, ConfId.ToString());
        }

        public void Initialized()
        {
        }

        public void Teardown()
        {
        }
    }
}
