using System;
using System.Globalization;
using Xamarin.Forms;

namespace moondraft.Converters
{
    public class ReadableByteSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteSize = (double)value;

            if (byteSize < 1024)
            {
                return $"{byteSize:F0}" + "B";
            }

            var units = new string[] { "KB", "MB", "GB", "TB" };
            var u = -1;
            do
            {
                byteSize /= 1024;
                ++u;
            } while (byteSize >= 1024 && u < units.Length - 1);
            return $"{byteSize:0.##}" + units[u];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
