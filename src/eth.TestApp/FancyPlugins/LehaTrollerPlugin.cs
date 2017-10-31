using eth.Eve.PluginSystem;
using eth.Telegram.BotApi.Objects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace eth.TestApp
{
    public class LehaTrollerPlugin : IPlugin
    {
        private const int LehinId = 292132197;
        //private const int LehinId = 114795281;

        private IPluginContext _ctx;

        private bool _isActive;

        public PluginInfo Info => new PluginInfo(new Guid("51b9ab14-7eab-4107-83bf-c9e50a2824f4"),
                                                         "LehaTroller",
                                                         "Leha Troller",
                                                         "0.0.0.1");

        public HandleResult Handle(IUpdateContext c)
        {
            if (c.IsInitiallyPolled)
                return HandleResult.Ignored;

            var message = c.Update.Message;

            string reply;
            InlineKeyboardMarkup ke;

            switch (message?.From.Id)
            {
                case LehinId:
                    if (!_isActive)
                        break;
                                        
                    reply = "а по-моему, ты лох";
                    ke = new InlineKeyboardMarkup
                    {
                        InlineKeyboard = new List<List<InlineKeyboardButton>>
                        {
                            new List<InlineKeyboardButton>
                            {
                                new InlineKeyboardButton { Text = "ты", CallbackData = "1" },
                                new InlineKeyboardButton { Text = "лох", CallbackData = "2" }
                            }
                        }
                    };

                    _ctx.BotApi.SendMessageAsync(chatId: message.Chat.Id, text: reply, replyToMessageId: message.MessageId, replyMarkup: ke);

                    return HandleResult.HandledCompletely;
                case 108762758:
                    if (Regex.IsMatch(message.Text ?? "", "хватит обижать леху", RegexOptions.IgnoreCase))
                        _isActive = false;
                    else if (Regex.IsMatch(message.Text ?? "", "леха зазнался", RegexOptions.IgnoreCase))
                        _isActive = true;
                    else
                        break;

                    reply = "как скажете, мой повелитель";

                    _ctx.BotApi.SendMessageAsync(chatId: message.Chat.Id, text: reply);

                    return HandleResult.HandledPartially;
            }

            if (Regex.IsMatch(message?.Text ?? "", "зазнался ли леха?"))
            {
                if (message.From.Id == LehinId)
                    reply = "да, леха, зазнался";
                else
                    reply = _isActive ? "определенно так" : "вроде пока нет......";

                _ctx.BotApi.SendMessageAsync(chatId: message.Chat.Id, text: reply);

                return HandleResult.HandledPartially;
            }

            return HandleResult.Ignored;
        }

        public void Initialize(IPluginContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialized() { }

        public void Teardown() { }
    }
}
