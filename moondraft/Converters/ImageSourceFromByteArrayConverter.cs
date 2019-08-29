using System;
using System.IO;
using Xamarin.Forms;

namespace moondraft.Converters
{
    public class ImageSourceFromByteArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource source = null;
            if (value != null)
            {
                byte[] byteArray = (byte[])value;
                source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            return source;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
