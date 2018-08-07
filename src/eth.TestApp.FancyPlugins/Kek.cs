using System;
using eth.Eve.PluginSystem;
using eth.Telegram.BotApi.Events;
using eth.Telegram.BotApi;
using System.Linq;
using System.Collections.Generic;
using eth.Telegram.BotApi.Objects;
using eth.PluginSamples;

namespace eth.TestApp.FancyPlugins
{
    internal class Kek : PluginBase, IRequestInterceptor, IResponseInterceptor
    {
        private readonly object _queueLock = new object();

        private readonly Queue<ResponseEventArgs> _messages = new Queue<ResponseEventArgs>();

        public override PluginInfo Info => throw new NotImplementedException();

        public override HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
                return HandleResult.Ignored;

            if (c.Update.Message?.ReplyToMessage?.From.Id != _ctx.Me.Id)
                return HandleResult.Ignored;

            if (c.Update.Message.Text != "?")
                return HandleResult.Ignored;

            var botMessage = c.Update.Message.ReplyToMessage;

            ResponseEventArgs storedMessageInfo;

            lock (_queueLock)
                storedMessageInfo = _messages.FirstOrDefault(m =>
                {
                    var msg = (Message)m.Response;
                    return msg.MessageId == botMessage.MessageId && msg.Chat.Id == botMessage.Chat.Id;
                });

            if (storedMessageInfo == null)
                _ctx.BotApi.SendMessageAsync(chatId: c.Update.Message.Chat.Id, replyToMessageId: c.Update.Message.MessageId, text: "i have no information about this message");
            else
                _ctx.BotApi.SendMessageAsync(chatId: c.Update.Message.Chat.Id, replyToMessageId: c.Update.Message.MessageId,
                    text: $"this message was sent by {storedMessageInfo.ApiOwner} via {storedMessageInfo.Method}");

            return HandleResult.HandledCompletely;
        }

        public void OnRequest(RequestEventArgs args)
        {
            if (args.Method != ApiMethod.SendMessage)
                return;

            var text = args.Arguments.First(a => a.ArgumentName == "text");

            text.Value = (string)text.Value + Environment.NewLine + Environment.NewLine + "sent by " + args.ApiOwner;
        }

        public void OnResponse(ResponseEventArgs args)
        {
            if (!(args.Response is Message))
                return;

            lock (_queueLock)
            {
                _messages.Enqueue(args);

                if (_messages.Count > 200)
                    _messages.Dequeue();
            }
        }
    }
}
