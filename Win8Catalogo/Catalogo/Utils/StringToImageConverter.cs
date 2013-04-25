using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Imaging;

namespace Win8Catalogo.Catalogo.Utils
{
    public class StringToImageConverter : IValueConverter
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage(new Uri(_baseUri, (string)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((BitmapImage)value).UriSource.MakeRelativeUri(_baseUri).ToString();
        }
    }
}
