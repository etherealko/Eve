using eth.Telegram.BotApi.Objects.Enums;
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
    }
}
