using eth.Eve.PluginSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eth.TestApp.YaDurak.ViewModels
{
    public class TelegramClientViewModel : ViewModelBase
    {
        private Dictionary<int, UserViewModel> _knownUsers = new Dictionary<int, UserViewModel>();

        private ChatViewModel _selectedChat;

        public ObservableCollection<ChatViewModel> Chats { get; } = new ObservableCollection<ChatViewModel>();

        public ChatViewModel SelectedChat { get => _selectedChat; set => SetField(ref _selectedChat, value); }

        public UserViewModel Me { get; }

        public TelegramClientViewModel(IPluginContext ctx)
        {
            Me = new UserViewModel(ctx.Me);

            _knownUsers[Me.Id] = Me;
        }

        public void Handle(IUpdateContext c)
        {
            if (c.Update.Message?.Chat == null)
                return;

            var chat = Chats.FirstOrDefault(ch => ch.Id == c.Update.Message.Chat.Id);
            int chatIndex;

            if (chat == null)
            {
                Chats.Add(chat = new ChatViewModel(c.Update.Message.Chat));
                chatIndex = Chats.Count - 1;
            }
            else
            {
                chatIndex = Chats.IndexOf(chat);
                chat.Update(c.Update.Message.Chat);
            }

            if (chatIndex != 0)
                Chats.Move(chatIndex, 0);

            var user = (UserViewModel)null;

            if (c.Update.Message?.From != null)            
                if (_knownUsers.TryGetValue(c.Update.Message.From.Id, out var knownUser))
                {
                    user = knownUser;
                    user.Update(c.Update.Message.From);
                }
                else
                {
                    user = new UserViewModel(c.Update.Message.From);
                    _knownUsers[user.Id] = user;
                }
            
            chat.Messages.Add(new MessageViewModel(c.Update.Message, user));
        }
    }
}
