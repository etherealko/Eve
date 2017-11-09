using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects.Enums;
using eth.TestApp.FancyPlugins;
using System;
using System.Collections.Generic;
using System.IO;
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
using IOPath = System.IO.Path;

#pragma warning disable CS4014 //missing await

namespace eth.TestApp
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UISupportPlugin _uiSupportPlugin;

        private readonly long _chatId;

        private DateTime _lastTextMessageIsTypingSent = DateTime.MinValue;

        public MainWindow(UISupportPlugin uiSupportPlugin)
        {
            _uiSupportPlugin = uiSupportPlugin;

            _chatId = _uiSupportPlugin.ChatId;

            Loaded += (o, e) => 
            {
                ActionComboBox.ItemsSource = Enum.GetValues(typeof(ChatAction));
                ActionComboBox.SelectedIndex = 0;

                TextMessageParseModeComboBox.ItemsSource = Enum.GetValues(typeof(ParseMode));
                TextMessageParseModeComboBox.SelectedIndex = 0;
            };

            InitializeComponent();
        }

        private void SendChatActionButton_Click(object sender, RoutedEventArgs e)
        {
            _uiSupportPlugin.PluginContext.BotApi.SendChatActionAsync(_chatId, (ChatAction)ActionComboBox.SelectedItem);
        }

        private async void PhotoDropBorder_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var photo = new InputFile(new FileStream(files[0], FileMode.Open, FileAccess.Read), "file" + IOPath.GetExtension(files[0]));

            _uiSupportPlugin.PluginContext.BotApi.SendChatActionAsync(_chatId, ChatAction.UploadingPhoto);
            var meh = await _uiSupportPlugin.PluginContext.BotApi.SendPhotoAsync(_chatId, photo);
        }

        private async void AudioDropBorder_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var audio = new InputFile(new FileStream(files[0], FileMode.Open, FileAccess.Read), "file" + IOPath.GetExtension(files[0]));

            _uiSupportPlugin.PluginContext.BotApi.SendChatActionAsync(_chatId, ChatAction.UploadingAudio);
            var meh = await _uiSupportPlugin.PluginContext.BotApi.SendAudioAsync(chatId: _chatId, audio: audio, 
                performer: AudioArtistTextBox.Text, title: AudioTitleTextBox.Text);
        }

        private void TextMessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextTypingEventCheckBox.IsChecked != true)
                return;

            if (string.IsNullOrEmpty(TextMessageTextBox.Text))
                return;

            if ((DateTime.Now - _lastTextMessageIsTypingSent) < TimeSpan.FromSeconds(3))
                return;

            _lastTextMessageIsTypingSent = DateTime.Now;

            _uiSupportPlugin.PluginContext.BotApi.SendChatActionAsync(_chatId, ChatAction.Typing);
        }

        private void TextMessageTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                return;

            e.Handled = true;
            SendTextMessageButton_Click(sender, null);
        }

        private async void SendTextMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var text = TextMessageTextBox.Text;

            if (string.IsNullOrWhiteSpace(text))
                return;

            TextMessageTextBox.IsEnabled = false;
            SendTextMessageButton.IsEnabled = false;

            var msg = await _uiSupportPlugin.PluginContext.BotApi.SendMessageAsync(_chatId, text, (ParseMode)TextMessageParseModeComboBox.SelectedItem);

            TextMessageTextBox.Text = "";
            TextMessageTextBox.IsEnabled = true;
            SendTextMessageButton.IsEnabled = true;
        }
    }
}
