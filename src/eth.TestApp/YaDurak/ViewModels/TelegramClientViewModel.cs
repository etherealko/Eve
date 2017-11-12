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
        public ObservableCollection<ChatViewModel> Chats { get; set; }

        public ChatViewModel SelectedChat { get; set; }
    }
}
