using System;
using System.Globalization;
using Xamarin.Forms;

namespace moondraft.Converters
{
    public class DateTimeOffsetFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTimeOffset)value).ToString(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTimeOffset.ParseExact(value as string, parameter as string, CultureInfo.InvariantCulture.DateTimeFormat);
        }
    }
}
