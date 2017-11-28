using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NokProjectX.Wpf.Converter
{
    public class HomeHoverConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool b)) return null;
            if (b)
            {
                return "/NokProjectX.Wpf;component/Resources/homeBlue.png";
            }
            return "/NokProjectX.Wpf;component/Resources/homeBlack.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}