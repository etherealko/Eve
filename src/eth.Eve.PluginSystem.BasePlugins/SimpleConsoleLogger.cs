using System;

namespace eth.Eve.PluginSystem.BasePlugins
{
    public class SimpleConsoleLogger : IPlugin
    {
        private IPluginContext _ctx;

        public PluginInfo Info { get; } = new PluginInfo(new Guid("51b9ab14-7eab-4107-83bf-c9e50a2824f4"),
                                                         "PluginOne",
                                                         "Brand new Eve plugin",
                                                         "0.0.0.1");

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Teardown()
        {
            Dispose();
        }

        public HandleResult Handle(IUpdateContext msg)
        {
            if (msg.IsInitiallyPolled)
                return HandleResult.Ignored;

            var chatMessage = msg.Update.Message;

            if (chatMessage == null)
                return HandleResult.Ignored;

            Console.WriteLine($"{chatMessage.Chat.Id}: {chatMessage.Text}");

            return HandleResult.HandledPartially;
        }

        public void Dispose() { }
    }
}
