using ExterniUredjajKlijentApp.Uposlenici.Helper;
using ExterniUredjajKlijentApp.Uposlenici.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ExterniUredjajKlijentApp.Uposlenici.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateUposlenik : Page { 
        public CameraHelper camera;
        public CreateUposlenik()
        {
            this.InitializeComponent();
            this.DataContext = new UposleniciViewModel();
            camera = new CameraHelper(PreviewControl);
            camera.InitializeCameraAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await camera.TakePhotoAsync();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(camera.Slika);
            SlikaControl.Source = bitmapImage;
        }
    }
}
