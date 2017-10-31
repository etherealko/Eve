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

namespace eth.TestApp.FancyPlugins
{
    /// <summary>
    /// Interaction logic for PhotoTextRenderControl.xaml
    /// </summary>
    public partial class PhotoTextRenderControl : UserControl
    {
        public string PhotoText { get; }
        public BitmapImage PhotoBitmap { get; }
                
        public PhotoTextRenderControl(byte[] imageFile, string text)
        {
            PhotoBitmap = new BitmapImage();
            PhotoText = text;
            
            using (var ms = new MemoryStream(imageFile))
            {
                PhotoBitmap.BeginInit();
                PhotoBitmap.CacheOption = BitmapCacheOption.OnLoad;
                PhotoBitmap.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                PhotoBitmap.StreamSource = ms;
                PhotoBitmap.EndInit();

                PhotoBitmap.Freeze();
            }

            InitializeComponent();
        }
    }
}
