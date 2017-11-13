using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eth.Telegram.BotApi.Objects;

namespace eth.TestApp.YaDurak.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private string _userName;

        private string _viewNameTemp;

        public int Id { get; }
        
        public string FirstName { get => _firstName; set => SetField(ref _firstName, value); }
        public string LastName { get => _lastName; set => SetField(ref _lastName, value); }
        public string UserName { get => _userName; set => SetField(ref _userName, value); }

        public string ViewNameTemp { get => _viewNameTemp; set => SetField(ref _viewNameTemp, value); }

        public UserViewModel(User user)
        {
            Id = user.Id;

            Update(user);
        }

        internal void Update(User fromUser)
        {
            FirstName = fromUser.FirstName;
            LastName = fromUser.LastName;
            UserName = fromUser.Username ?? fromUser.Id.ToString();
            ViewNameTemp = $"{FirstName} {LastName} ({UserName})";
        }
    }
}
