using moondraft.Services;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace moondraft.Converters
{
    public class IsImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DetectFileTypeService.IsImage(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
