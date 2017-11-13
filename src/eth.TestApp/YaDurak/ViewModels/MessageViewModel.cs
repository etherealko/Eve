using eth.Telegram.BotApi.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eth.TestApp.YaDurak.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private bool _isSent;

        public UserViewModel Sender { get; }

        public bool IsMyMessage { get; }
        public bool IsSent { get => _isSent; set => SetField(ref _isSent, value); }

        public string Text { get; private set; }

        public MessageViewModel(Message message, UserViewModel sender)
        {
            Sender = sender;

            Text = message.Text ?? JsonConvert.SerializeObject(message);
        }

        public MessageViewModel(string text, UserViewModel me)
        {
            Sender = me;

            IsMyMessage = true;
            Text = text;
        }

        public void OnSent(Message message)
        {
            IsSent = true;
        }

        public void OnSendFailed(Exception ex)
        {

        }
    }
}
