using eth.Telegram.BotApi;
using eth.Telegram.BotApi.Objects.Enums;
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

namespace eth.TestApp
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UISupportPlugin _uiSupportPlugin;

        public MainWindow(UISupportPlugin uiSupportPlugin)
        {
            _uiSupportPlugin = uiSupportPlugin;

            Loaded += (o, e) => 
            {
                ActionComboBox.ItemsSource = Enum.GetValues(typeof(ChatAction));
                ActionComboBox.SelectedIndex = 0;
            };

            InitializeComponent();
        }

        private void SendChatActionButton_Click(object sender, RoutedEventArgs e)
        {
            _uiSupportPlugin.PluginContext.BotApi.SendChatActionAsync(_uiSupportPlugin.ChatId, (ChatAction)ActionComboBox.SelectedItem);
        }

        private async void PhotoDropBorder_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var photo = new InputFile(new FileStream(files[0], FileMode.Open, FileAccess.Read), "file" + IOPath.GetExtension(files[0]));

            var meh = await _uiSupportPlugin.PluginContext.BotApi.SendPhotoAsync(_uiSupportPlugin.ChatId, photo);
        }

        private async void AudioDropBorder_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            var audio = new InputFile(new FileStream(files[0], FileMode.Open, FileAccess.Read), "file" + IOPath.GetExtension(files[0]));
            
            var meh = await _uiSupportPlugin.PluginContext.BotApi.SendAudioAsync(chatId: _uiSupportPlugin.ChatId, audio: audio, 
                performer: AudioArtistTextBox.Text, title: AudioTitleTextBox.Text);
        }
    }
}
