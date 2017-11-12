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
        public UserViewModel Sender { get; private set; }

        public bool IsSent { get; set; }

        public string Text { get; private set; }
    }
}
