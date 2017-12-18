using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NokProjectX.Wpf.Converter
{
    public class BoolNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool b)
            {
                flag = b;
            }
            return !flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}