using eth.PluginSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eth.Eve.PluginSystem;

namespace eth.TestApp.FancyPlugins
{
    internal class HealthStatusPlugin : PluginBase
    {
        private long HealthStatusChatId;

        public override PluginInfo Info => new PluginInfo(new Guid("81AC888A-0696-4682-BB4E-C3C45612E72B"), "HealthStatus", "Health status plugin", "0.0.0.1");

        public override void Initialize(IPluginContext ctx)
        {
            base.Initialize(ctx);

            _ctx.BotApi.HttpClientTimeout = TimeSpan.FromSeconds(30);

            if (!_ctx.GetStorage().TryGetString("healthStatusChatId", out var value))
                throw new Exception("healthStatusChatId is not defined");

            HealthStatusChatId = long.Parse(value.Value);

            SendHealthStatusMessage("this.Initialize()", true);
        }

        public override void Initialized()
        {
            SendHealthStatusMessage("this.Initialized()", true);
        }

        public override void Teardown()
        {
            try
            {
                SendHealthStatusMessage("this.Teardown()", true);
            }
            catch { }
        }

        public override HandleResult Handle(IUpdateContext c)
        {
            if (c.Update.Message?.Chat?.Id != HealthStatusChatId)
                return HandleResult.Ignored;
            
            return HandleResult.HandledCompletely;
        }

        private void SendHealthStatusMessage(string text, bool wait = false)
        {
            var task = _ctx.BotApi.SendMessageAsync(HealthStatusChatId, $"{DateTime.Now:HH:mm:ss.fff}: {text}");

            if (wait)
                task.Wait();
        }
    }
}
