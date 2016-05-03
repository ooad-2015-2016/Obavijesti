using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace ProjekatOoad.Helper
{
    //Convertuje string url Slika u BitmapImage koji prihvata image kontrola
    class StringUrlToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || !(value is string))
                return null;
                //Bitmap image se moze napraviti od url-a koji je na internetu
                //Ako nema konekcije nece biti ni slike 
                BitmapImage image = new BitmapImage(new Uri((string)value, UriKind.Absolute));
                return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
