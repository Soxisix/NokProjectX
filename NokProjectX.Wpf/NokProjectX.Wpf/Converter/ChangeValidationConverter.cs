using System;
using System.Globalization;
using System.Windows.Data;

namespace NokProjectX.Wpf.Converter
{
    public class ChangeValidationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) < 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}