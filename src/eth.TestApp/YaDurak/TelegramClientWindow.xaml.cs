using eth.Eve.PluginSystem;
using eth.TestApp.YaDurak.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eth.TestApp.YaDurak
{
    /// <summary>
    /// Interaction logic for TelegramClientWindow.xaml
    /// </summary>
    public partial class TelegramClientWindow : Window
    {
        private readonly TelegramClientPlugin _telegramClientPlugin;

        public TelegramClientViewModel TelegramClientViewModel { get; set; }

        public TelegramClientWindow(TelegramClientPlugin telegramClientPlugin)
        {
            _telegramClientPlugin = telegramClientPlugin;
            _telegramClientPlugin.HandleEvent += TelegramClientPlugin_HandleEvent;
            
            InitializeComponent();
        }

        private void TelegramClientPlugin_HandleEvent(object sender, HandleEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Handle(IUpdateContext c)
        {

        }
    }
}
