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

            TelegramClientViewModel = new TelegramClientViewModel(telegramClientPlugin.PluginContext);

            InitializeComponent();
        }

        private void TelegramClientPlugin_HandleEvent(object sender, IUpdateContext c)
        {
            Dispatcher.BeginInvoke((Action)(() => { TelegramClientViewModel.Handle(c); }));
        }

        private void TextMessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextMessageTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                return;

            e.Handled = true;
            SendTextMessageButton_Click(sender, null);
        }

        //move to viewmodel?
        private async void SendTextMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var text = TextMessageTextBox.Text;

            if (string.IsNullOrWhiteSpace(text) || TelegramClientViewModel.SelectedChat == null)
                return;

            TextMessageTextBox.Text = "";
            TextMessageTextBox.Focus();

            var unsentMsg = new MessageViewModel(text, TelegramClientViewModel.Me);
            TelegramClientViewModel.SelectedChat.Messages.Add(unsentMsg);

            var chatIndex = TelegramClientViewModel.Chats.IndexOf(TelegramClientViewModel.SelectedChat);

            if (chatIndex != 0)
                TelegramClientViewModel.Chats.Move(chatIndex, 0);

            try
            {
                var sentMsg = await _telegramClientPlugin.PluginContext.BotApi.SendMessageAsync(TelegramClientViewModel.SelectedChat.Id, text);
                unsentMsg.OnSent(sentMsg);
            }
            catch (Exception ex)
            {
                unsentMsg.OnSendFailed(ex);
            }
        }
    }
}
