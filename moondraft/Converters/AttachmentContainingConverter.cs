using System;
using System.Globalization;
using Xamarin.Forms;

namespace moondraft.Converters
{
    public class AttachmentContainingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
