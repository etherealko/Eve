using eth.Telegram.BotApi.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eth.TestApp.YaDurak.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private string _name;
        private string _draftMessage;

        public long Id { get; }

        public string Name { get => _name; set => SetField(ref _name, value); }

        public ObservableCollection<MessageViewModel> Messages { get; }

        public string DraftMessage { get => _draftMessage; set => SetField(ref _draftMessage, value); }

        public ChatViewModel(Chat chat)
        {
            Id = chat.Id;

            Update(chat);

            Messages = new ObservableCollection<MessageViewModel>();
        }

        public void Update(Chat fromChat)
        {
            Name = fromChat.Title ?? $"{fromChat.FirstName} {fromChat.LastName}";
        }
    }
}
